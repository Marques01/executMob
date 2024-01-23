namespace Domain.Entities
{
    public class Roles
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<User>? Users { get; set; }
    }
}
