using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetUsersAsync();
    }
}
