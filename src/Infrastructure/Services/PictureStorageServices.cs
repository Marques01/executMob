using Domain.Entities.Response;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.JSInterop;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class PictureStorageServices : IPictureStorageServices
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _js;

        private readonly HeadersMethods _headersMethods;

        public PictureStorageServices(HttpClient client, IJSRuntime js)
        {
            _httpClient = client;
            _js = js;
            _headersMethods = new(_httpClient, _js);
        }
        public async Task<PictureSaveResponseModel> UploadAsync(MultipartFormDataContent multipartFormDataContent)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var response = await _httpClient.PostAsync("api/files/upload", multipartFormDataContent);

                string content = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var responseModel = JsonSerializer.Deserialize<PictureSaveResponseModel>(content, options);

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
