using Domain.Entities.Request;
using Domain.Entities.Response;

namespace Domain.Interfaces
{
    public interface IAccountServices
    {
        Task<UserTokenResponseModel> SignInAsync(UserRequestModel userDto);
    }
}
