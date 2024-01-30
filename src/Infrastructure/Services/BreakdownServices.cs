using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class BreakdownServices : IBreakdownServices
    {
        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private readonly HeadersMethods _headersMethods;

        public BreakdownServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _headersMethods = new HeadersMethods(httpClient, jsRuntime);            
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;            
        }

        public async Task CreateAsync(Breakdown breakdown)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Breakdown breakdown)
        {
            throw new NotImplementedException();
        }

        public async Task<Breakdown> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<Breakdown> GetBreakdownsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = await _headersMethods.SetTokenHeaderAuthorization();

            var vehicles = _httpClient.GetFromJsonAsAsyncEnumerable<Breakdown>("api/breakdown");

            await foreach (var vehicle in vehicles)
            {
                await Task.Delay(1200);

                yield return vehicle ?? new();
            }
        }

        public async Task<IEnumerable<Breakdown>> GetBreakdownsAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int vehicleId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
