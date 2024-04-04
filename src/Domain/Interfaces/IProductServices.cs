using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
