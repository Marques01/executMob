using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserServices
    {
        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task<bool> UserExistingAsync(string login);

        Task<User> GetUserByIdAsync(int id);

        Task<User> SignInAsync(string login, string password);

        Task<User> GetUserByLoginAsync(string mail);

        Task<IEnumerable<User>> GetUsersAsync();
    }
}
