using System.Net;
using System.Text.Json;
using ArsitekturBawang.Application.Services;
using ArsitekturBawang.Application.Services.Contracts;
using ArsitekturBawang.Domain.Exceptions;
using ArsitekturBawang.Domain.Repositories;
using ArsitekturBawang.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Persistence Service
builder.Services.AddDbContext<IDataContext, DataContext>(config =>
    config.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

// Application Service
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto Generate Database
    var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
    var db = serviceScope?.ServiceProvider.GetRequiredService<DataContext>();
    db?.Database.EnsureCreated();
}

// Global Error Handler
app.UseExceptionHandler(app =>
{
    app.Run(async context =>
    {
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature == null) return;

        context.Response.StatusCode = contextFeature.Error switch
        {
            OperationCanceledException => (int)HttpStatusCode.ServiceUnavailable,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var errorResponse = new
        {
            statusCode = context.Response.StatusCode,
            message = contextFeature.Error.GetBaseException().Message
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    });


});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
