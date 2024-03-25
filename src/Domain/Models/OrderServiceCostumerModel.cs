using Domain.Enum;

namespace Domain.Models
{
    public class OrderServiceCostumerModel
    {
        public int BreakdownId { get; set; }

        public StatusEnum Status { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
