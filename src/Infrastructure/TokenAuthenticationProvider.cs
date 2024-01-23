using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.JSInterop;
using Domain.Interfaces;
using Infrastructure.Extensions;

namespace Infrastructure
{
    public class TokenAuthenticationProvider : AuthenticationStateProvider, IAuthorizeServices
    {
        private readonly IJSRuntime _js;
        private readonly HttpClient _client;

        public static readonly string TokenKey = "tokenKey";

        public TokenAuthenticationProvider(IJSRuntime jsRunTime, HttpClient client)
        {
            _js = jsRunTime;

            _client = client;
        }

        private AuthenticationState NotAuthenticate()
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private AuthenticationState CreateAuthenticationState(string token)
        {
            return new AuthenticationState(new ClaimsPrincipal
                (new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.GetFromLocalStorage(TokenKey);

            if (string.IsNullOrEmpty(token)) return NotAuthenticate();

            return CreateAuthenticationState(token);
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();

            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            Dictionary<string, object> keyValuePairs = new();

            if (jsonBytes != null)
            {
                keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;

                if (keyValuePairs != null)
                {
                    claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
                }
            }

            return claims;
        }

        public async Task LoginAsync(string token)
        {
            try
            {
                await _js.SetInLocalStorage(TokenKey, token);
                var authState = CreateAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível fazer login\n {ex.Message}");
            }
        }

        public async Task Logout()
        {
            try
            {
                await _js.RemoveItem(TokenKey);
                _client.DefaultRequestHeaders.Authorization = null;
                NotifyAuthenticationStateChanged(Task.FromResult(NotAuthenticate()));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
