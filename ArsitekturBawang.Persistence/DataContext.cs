using ArsitekturBawang.Domain.Entities;
using ArsitekturBawang.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArsitekturBawang.Persistence;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    public IQueryable<T> GetQuery<T>() where T : class
    {
        return Set<T>().AsQueryable();
    }

    public void Create<T>(T entity) where T : class
    {
        Set<T>().Add(entity);
    }

    public new void Update<T>(T entity) where T : class
    {
        Set<T>().Update(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        Set<T>().Remove(entity);
    }

    public async Task<int> Save(CancellationToken cancellationToken)
    {
        return await SaveChangesAsync(cancellationToken);
    }
}

