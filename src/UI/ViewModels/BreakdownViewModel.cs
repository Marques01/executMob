using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels
{
    public class BreakdownViewModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int OdometerStart { get; set; }

        [Required]
        public string Description { get; set; } = "N/A";
    }
}
