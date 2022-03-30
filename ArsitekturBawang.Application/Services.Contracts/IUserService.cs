using ArsitekturBawang.Application.Resources;

namespace ArsitekturBawang.Application.Services.Contracts;

public interface IUserService
{
    Task<UserResource> Get(int id, CancellationToken cancellationToken);
    Task<List<UserResource>> GetAll(CancellationToken cancellationToken);
    Task<UserResource> Create(UserResource resource, CancellationToken cancellationToken);
    Task<UserResource> Update(UserResource resource, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
}

