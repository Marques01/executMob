using Domain.Entities;
using UI.ViewModels;

namespace UI.Components.Pages.Breakdown
{
    public partial class Create
    {
        private bool
            _isLoading = false,
            _showValidationLogin = false,
            _showValidationPassword = false;

        private BreakdownViewModel _breakdownViewModel = new();

        private IEnumerable<Vehicle> _vehicles = Enumerable.Empty<Vehicle>();

        protected override async Task OnInitializedAsync()
        {
            await GetVehiclesAsync();
        }

        private async Task HandleSubmit()
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task GetVehiclesAsync()
        {
            try
            {
                _isLoading = true;

                await Task.Delay(4000);

                //_vehicles = await _vehicleServices.GetVehiclesAsync();

                _vehicles = new List<Vehicle>
                {
                    new Vehicle
                    {
                        VehicleId = 1,
                        Plate = "ABC-123",
                        Model = "Fiat Uno",
                        CreatedAt = DateTime.Now,
                        Enabled = true
                    },
                    new Vehicle
                    {
                        VehicleId = 2,
                        Plate = "ABC-456",
                        Model = "Fiat Palio",
                        CreatedAt = DateTime.Now,
                        Enabled = true
                    },
                    new Vehicle
                    {
                        VehicleId = 3,
                        Plate = "ABC-789",
                        Model = "Fiat Toro",
                        CreatedAt = DateTime.Now,
                        Enabled = true
                    }
                };
              

                _isLoading = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ShowErrorAsync(string message, string? title = null) => await _dialogServices.ShowErrorAsync(message, title);

        private void ValidationLoginMessage() => _showValidationLogin = string.IsNullOrEmpty(_breakdownViewModel.Description);

        private void ValidationPasswordMessage() => _showValidationPassword = string.IsNullOrEmpty(_breakdownViewModel.Description);
    }
}
