namespace Domain.Models
{
    public class BreakdownCostumerModel
    {
        public int VehicleId { get; set; }
        
        public int OdometerStart { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
