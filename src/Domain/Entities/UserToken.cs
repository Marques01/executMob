using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public record UserToken
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; } = string.Empty;

        [JsonPropertyName("token")]
        public string Token { get; init; } = string.Empty;

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; init; }
    }
}
