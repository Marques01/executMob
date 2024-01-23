namespace Domain.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        public string Plate { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool Enabled { get; set; }

        public ICollection<Breakdown>? Breakdowns { get; set; }
    }
}
