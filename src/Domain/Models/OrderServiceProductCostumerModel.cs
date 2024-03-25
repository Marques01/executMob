using Domain.Entities;

namespace Domain.Models
{
    public class OrderServiceProductCostumerModel
    {
        public int OrderServiceId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
