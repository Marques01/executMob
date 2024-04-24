using Domain.Entities.Response;

namespace Domain.Interfaces
{
    public interface IPictureStorageServices
    {
        Task<PictureSaveResponseModel> UploadAsync(MultipartFormDataContent multipartFormDataContent);

        Task<string> GetBase64Async(string pictureName);
    }
}
