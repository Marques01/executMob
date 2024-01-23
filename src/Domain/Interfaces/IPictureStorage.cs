using Domain.Dto;

namespace Domain.Interfaces
{
    public interface IPictureStorage
    {
        Task<PictureSaveDto> UploadAsync(Stream stream, string fileName);
    }
}
