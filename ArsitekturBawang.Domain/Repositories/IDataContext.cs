namespace ArsitekturBawang.Domain.Repositories;

public interface IDataContext
{
    IQueryable<T> GetQuery<T>() where T : class;
    void Create<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<int> Save(CancellationToken cancellationToken);
}