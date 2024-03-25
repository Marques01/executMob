using Domain.Entities.Response;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IOrderServiceProcessing
    {
        Task<OrderServiceResponseModel> CreateAsync(OrderServiceCostumerModel orderService);

        Task<OrderServiceResponseModel> UpdateAsync(OrderServiceCostumerModel orderService);

        Task<OrderServiceResponseModel> GetOrderServiceByIdAsync(int id);

        Task<OrderServiceProductResponseModel> AddProductAsync(OrderServiceProductCostumerModel orderServiceProduct);

        Task<OrderServiceProductResponseModel> RemoveProductAsync(int id);

        Task<OrderServiceProductResponseModel> GetOrderServiceProductByIdAsync(int id);

        Task<OrderServiceResponseModel> CloseOrderServiceAsync(int id);
    }
}
