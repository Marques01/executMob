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

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var users = await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/user");

                return users ?? Enumerable.Empty<User>();
            }
            catch (Exception)
            {
                throw new Exception(
                    "Ocorreu um erro ao obter a lista de usuários. Caso o problema persistir entre em contato com o administrado do sistema");
            }
        }
    }
}
