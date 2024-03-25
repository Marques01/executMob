namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<OrderServiceProduct>? OrderServiceProducts { get; set; }
    }
}
