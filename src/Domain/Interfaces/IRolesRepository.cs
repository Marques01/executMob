using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRolesRepository
    {
        Task CreateAsync(Roles roles);
    }
}
