namespace Domain.Interfaces
{
    public interface IAuthorizeServices
    {
        Task LoginAsync(string token);

        Task Logout();
    }
}
