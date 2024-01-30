using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IVehicleServices
    {
        Task CreateAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);

        Task<IEnumerable<Vehicle>> GetVehiclesAsync();

        Task<bool> VehicleExistsAsync(string plate);

        Task<Vehicle> GetVehicleByIdAsync(int id);
    }
}
