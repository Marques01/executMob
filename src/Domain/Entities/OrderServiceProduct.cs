namespace Domain.Entities
{
    public class OrderServiceProduct
    {
        public int Id { get; set; }

        public int OrderServiceId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; } = string.Empty;

        public OrderService? OrderService { get; set; }

        public Product? Product { get; set; }
    }
}
