using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities.Response;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.JSInterop;

namespace Infrastructure.Services
{
    public class OrderServiceProcessing : IOrderServiceProcessing
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public OrderServiceProcessing(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _headersMethods = new HeadersMethods(httpClient, jsRuntime);
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<OrderServiceResponseModel> CreateAsync(OrderServiceCostumerModel orderService)
        {
            try
            {
                await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var response = await _httpClient.PostAsJsonAsync("api/OrderService", orderService);

                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

                var responseModel = JsonSerializer.Deserialize<OrderServiceResponseModel>(content, options);

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Erro ao tentar criar a OS.");
            }
            catch (ArgumentException arg)
            {
                throw new ArgumentException(arg.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<OrderServiceResponseModel> UpdateAsync(OrderServiceCostumerModel orderService)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceResponseModel> GetOrderServiceByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceProductResponseModel> AddProductAsync(OrderServiceProductCostumerModel orderServiceProduct)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceProductResponseModel> RemoveProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceProductResponseModel> GetOrderServiceProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceResponseModel> CloseOrderServiceAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
