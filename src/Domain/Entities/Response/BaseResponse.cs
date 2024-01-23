using System.Net;

namespace Domain.Entities.Response
{
    public record BaseResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }

        public bool IsSuccess { get; init; }

        public string Message { get; init; } = string.Empty;

        public virtual T? Model { get; init; }
    }
}
