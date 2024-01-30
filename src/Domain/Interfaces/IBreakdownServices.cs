using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBreakdownServices
    {
        Task CreateAsync(Breakdown breakdown);

        Task UpdateAsync(Breakdown breakdown);

        Task<Breakdown> GetAsync(int id);

        IAsyncEnumerable<Breakdown> GetBreakdownsAsync();

        Task<IEnumerable<Breakdown>> GetBreakdownsAsync(int page, int pageSize);

        Task<bool> ExistsAsync(int vehicleId, DateTime date);
    }
}
