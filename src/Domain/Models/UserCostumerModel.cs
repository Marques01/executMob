namespace Domain.Models
{
    public record UserCostumerModel
    {
        public string Name { get; init; } = string.Empty;

        public string Login { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;

        public int RoleId { get; init; }
    }
}
