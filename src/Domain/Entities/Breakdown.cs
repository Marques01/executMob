namespace Domain.Entities
{
    public class Breakdown
    {
        public int BreakdownId { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public int VehicleId { get; set; }

        public int OdometerStart { get; set; }

        public int OdometerEnd { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<BreakdownImages>? BreakdownImages { get; set; }

        public Vehicle? Vehicle { get; set; }

        public OrderService? OrderService { get; set; }

        public ICollection<BreakdownUser>? BreakdownUsers { get; set; }
    }
}
