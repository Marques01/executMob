using Domain.Enum;

namespace Domain.Entities
{
    public class OrderService
    {
        public int OrderServiceId { get; set; }

        public string Description { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public int BreakDownId { get; set; }

        public Breakdown? Breakdown { get; set; }

        public ICollection<OrderServiceProduct>? OrderServiceProducts { get; set; }
    }
}
