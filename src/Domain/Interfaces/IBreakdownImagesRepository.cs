using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBreakdownImagesRepository
    {
        Task CreateAsync(BreakdownImages breakdownImages);

        Task DeleteAsync(int id);
    }
}
