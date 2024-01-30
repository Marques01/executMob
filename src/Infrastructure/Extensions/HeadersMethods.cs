using Infrastructure;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Infrastructure.Extensions
{
    public class HeadersMethods
    {
        private readonly HttpClient _client;

        private readonly IJSRuntime _js;

        public HeadersMethods(HttpClient client, IJSRuntime js)
        {
            _client = client;

            _js = js;
        }

        public async Task<AuthenticationHeaderValue> SetTokenHeaderAuthorizationAsync()
        {
            var token = await _js.GetFromLocalStorage(TokenAuthenticationProvider.TokenKey);

            var headerValue = _client.DefaultRequestHeaders.Authorization = new("bearer", token);

            return headerValue;
        }
    }
}
