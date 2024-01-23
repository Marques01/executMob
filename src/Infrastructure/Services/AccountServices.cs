using Domain.Entities.Request;
using Domain.Entities.Response;
using Domain.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class AccountServices : IAccountServices
    {
        private HttpClient _client;

        public AccountServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserTokenResponseModel> SignInAsync(UserRequestModel userRequest)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/account/signin", userRequest);

                string? content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

                    var user = JsonSerializer.Deserialize<UserTokenResponseModel>(content, options);

                    if (user != null)
                        return user;
                }

                throw new Exception("Ocorreu um erro ao solicitar a requisição. Tente novamente mais tarde");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
