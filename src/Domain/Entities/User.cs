namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Enabled { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime LastLogin { get; set; }

        public int RoleId { get; set; }

        public Roles? Role { get; set; }
    }
}