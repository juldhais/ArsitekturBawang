using ArsitekturBawang.Application.Resources;
using ArsitekturBawang.Application.Services.Contracts;
using ArsitekturBawang.Domain.Entities;
using ArsitekturBawang.Domain.Exceptions;
using ArsitekturBawang.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArsitekturBawang.Application.Services;

public class UserService : IUserService
{
    private readonly IDataContext _db;

    public UserService(IDataContext db)
    {
        _db = db;
    }

    public async Task<UserResource> Get(int id, CancellationToken cancellationToken)
    {
        var user = await _db.GetQuery<User>()
            .Where(x => x.Id == id)
            .Select(x => new UserResource
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found.");

        return user;
    }

    public async Task<List<UserResource>> GetAll(CancellationToken cancellationToken)
    {
        return await _db.GetQuery<User>()
            .Select(x => new UserResource
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);
    }

    private void Validate(UserResource resource)
    {
        if (string.IsNullOrWhiteSpace(resource.Name))
            throw new BadRequestException("Name cannot be empty.");
    }

    public async Task<UserResource> Create(UserResource resource, CancellationToken cancellationToken)
    {
        Validate(resource);

        var user = new User
        {
            Name = resource.Name
        };
        _db.Create(user);
        await _db.Save(cancellationToken);

        return await Get(user.Id, cancellationToken);
    }

    public async Task<UserResource> Update(UserResource resource, CancellationToken cancellationToken)
    {
        Validate(resource);

        var user = await _db.GetQuery<User>().FirstOrDefaultAsync(x => x.Id == resource.Id, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found.");
        
        user.Name = resource.Name;

        _db.Update(user);
        await _db.Save(cancellationToken);

        return await Get(user.Id, cancellationToken);
    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var user = await _db.GetQuery<User>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found.");

        _db.Delete<User>(user);
        await _db.Save(cancellationToken);
    }
}

