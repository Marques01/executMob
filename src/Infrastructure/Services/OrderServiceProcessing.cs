using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.Entities;
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

        public async Task<OrderServiceProductResponseModel> AddProductAsync(OrderServiceProductCostumerModel orderServiceProduct)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(orderServiceProduct), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/OrderService/AddProduct", httpContent);

                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var responseModel = JsonSerializer.Deserialize<OrderServiceProductResponseModel>(content, options);

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Ocorreu um erro em nossos servidores. Tente novamente mais tarde");
            }
            catch (ArgumentNullException arg)
            {
                throw new ArgumentException(arg.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<OrderServiceProductResponseModel> RemoveProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderServiceProductResponseModel> GetOrderServiceProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderServiceResponseModel> CloseOrderServiceAsync(int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();
                
                var response = await _httpClient.PutAsync($"api/OrderService/CloseOS/{id}", null);

                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var responseModel = JsonSerializer.Deserialize<OrderServiceResponseModel>(content, options);

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Ocorreu um erro em nossos servidores. Tente novamente mais tarde");
            }
            catch (ArgumentNullException arg)
            {
                throw new ArgumentException(arg.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
