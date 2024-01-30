using System.Net.Http.Json;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.JSInterop;

namespace Infrastructure.Services
{
    public class VehicleServices : IVehicleServices
    {
        private readonly HttpClient _httpClient;

        private IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public VehicleServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _headersMethods = new HeadersMethods(httpClient, jsRuntime);
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task CreateAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorizationAsync();

                var vehicles = await _httpClient.GetFromJsonAsync<IEnumerable<Vehicle>>("api/vehicle");

                return vehicles ?? Enumerable.Empty<Vehicle>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VehicleExistsAsync(string plate)
        {
            throw new NotImplementedException();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
