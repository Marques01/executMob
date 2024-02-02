using Domain.Entities;
using Domain.Entities.Response;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBreakdownServices
    {
        Task<BreakdownResponseModel> CreateAsync(BreakdownCostumerModel breakdown);

        IAsyncEnumerable<Breakdown> GetBreakdownsAsync();

        Task<IEnumerable<Breakdown>> GetBreakdownsAsync(int page, int pageSize);

        Task<BreakdownImagesResponseModel> SaveDirectoryImage(BreakdownImages breakdownImages);
    }
}
