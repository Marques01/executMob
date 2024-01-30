using System.Net.Http.Json;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.JSInterop;

namespace Infrastructure.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public UserServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _headersMethods = new HeadersMethods(httpClient, jsRuntime);
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistingAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> SignInAsync(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByLoginAsync(string mail)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorization();

                var users = await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/user");

                return users ?? Enumerable.Empty<User>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
