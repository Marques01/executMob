using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public ProductServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _headersMethods = new HeadersMethods(_httpClient, _jsRuntime);
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var response = await _httpClient.GetAsync("api/Product/all");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var responseModel = JsonSerializer.Deserialize<IEnumerable<Product>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (responseModel is not null)
                        return responseModel;
                }

                throw new Exception("Erro ao tentar buscar os produtos.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
