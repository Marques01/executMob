using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task CreateAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);

        IAsyncEnumerable<Vehicle> GetVehiclesAsync();

        Task<bool> VehicleExistsAsync(string plate);

        Task<Vehicle> GetVehicleByIdAsync(int id);
    }
}
