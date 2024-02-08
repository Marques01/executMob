namespace Domain.Entities
{
    public class BreakdownUser
    {
        public int Id { get; set; }

        public int BreakdownId { get; set; }

        public int UserId { get; set; }

        public Breakdown? Breakdown { get; set; }

        public User? User { get; set; }
    }
}
