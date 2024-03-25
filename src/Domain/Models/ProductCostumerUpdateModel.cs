namespace Domain.Models
{
    public class ProductCostumerUpdateModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
