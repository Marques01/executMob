using Domain.Entities;
using Domain.Entities.Response;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class BreakdownServices : IBreakdownServices
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public BreakdownServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _headersMethods = new HeadersMethods(httpClient, jsRuntime);
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<BreakdownResponseModel> CreateAsync(BreakdownCostumerModel breakdown)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                string jsonData = JsonSerializer.Serialize(breakdown);

                var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/breakdown", httpContent);

                string content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<BreakdownResponseModel>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Erro ao tentar criar a avaria.");
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

        public async IAsyncEnumerable<Breakdown> GetBreakdownsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

            var vehicles = _httpClient.GetFromJsonAsAsyncEnumerable<Breakdown>("api/breakdown");

            await foreach (var vehicle in vehicles)
            {
                yield return vehicle ?? new();
            }
        }

        public async Task<IEnumerable<Breakdown>> GetBreakdownsIEnumerableAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var vehicles = await _httpClient.GetFromJsonAsync<IEnumerable<Breakdown>>("api/breakdown");

                return vehicles ?? Enumerable.Empty<Breakdown>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<IEnumerable<Breakdown>> GetBreakdownsAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<BreakdownImagesResponseModel> SaveDirectoryImage(BreakdownImages breakdownImages)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                string jsonData = JsonSerializer.Serialize(breakdownImages);

                var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/breakdown/save/image", httpContent);

                string content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<BreakdownImagesResponseModel>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Erro ao associar a imagem da avaria.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Breakdown> GetBreakdownAsync(int id)
        {
            try
            {
                await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var breakdownResponseModel =
                    await _httpClient.GetFromJsonAsync<BreakdownResponseModel>($"api/breakdown/{id}");

                if (breakdownResponseModel is not null)
                {
                    if (breakdownResponseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(breakdownResponseModel.Message);

                    if (breakdownResponseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(breakdownResponseModel.Message);

                    return breakdownResponseModel.Model!;
                }

                throw new Exception("Erro ao tentar buscar a avaria.");
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

        public async Task<BreakdownUserResponseModel> AssociateUser(BreakdownUserCostumerModel costumerModel)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                string jsonData = JsonSerializer.Serialize(costumerModel);

                var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/breakdown/associate/user", httpContent);

                string content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<BreakdownUserResponseModel>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Erro ao associar a imagem da avaria.");
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

        public async Task<BreakdownResponseModel> UpdateAsync(BreakdownCostumerUpdateModel breakdown)
        {
            try
            {
                await _headersMethods.SetTokenHeaderAuthorizationAsync();

                string jsonData = JsonSerializer.Serialize(breakdown);

                var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/breakdown/change", httpContent);

                string content = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<BreakdownResponseModel>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (responseModel is not null)
                {
                    if (responseModel.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(responseModel.Message);

                    if (responseModel.StatusCode == HttpStatusCode.BadRequest)
                        throw new ArgumentException(responseModel.Message);

                    return responseModel;
                }

                throw new Exception("Erro ao tentar atualizar a avaria.");
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
    }
}
