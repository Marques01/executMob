using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Fast.Components.FluentUI;
using UI.Components.Layout;
using UI.ViewModels;

namespace UI.Components.Pages.Breakdown
{
    public partial class Create
    {
        private bool
            _isLoading = false,
            _showValidationOdometer = false;

        private string
            _inputOdometerClass = string.Empty;

        private BreakdownViewModel _breakdownViewModel = new();

        private List<Vehicle> _vehicles = new();

        private List<User> _users = new();

        private List<User> _usersList = new();

        private List<string> _base64List = new();

        private BrowserFiles _browserFiles = new();

        private MultipartFormDataContent _content = new();

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            await GetVehiclesAsync();

            await GetUsersAsync();

            _isLoading = false;
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
                var vehicles = await _vehicleServices.GetVehiclesAsync();

                _vehicles = vehicles.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task GetUsersAsync()
        {
            try
            {
                var users = await _userServices.GetUsersAsync();

                _users = users.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void ChkEmployeeChanged(ChangeEventArgs e)
        {
            if (e.Value is string[] usersIds)
            {
                usersIds = usersIds.Take(2).ToArray();

                var userRemove = _usersList.Where(x => !usersIds.Contains(x.UserId.ToString())).ToList();

                foreach (var userId in usersIds)
                {
                    var user = _users.First(x => x.UserId == int.Parse(userId));

                    bool exists = _usersList.Exists(x => x.UserId == user.UserId);

                    if (!exists)
                        _usersList.Add(user);
                }

                foreach (var user in userRemove)
                {
                    _usersList.Remove(user);
                }
            }
        }

        private void OnVehicleChanged(ChangeEventArgs e)
        {
        }

        private void ValidationOdotometerMessage()
        {
            if (_breakdownViewModel.OdometerStart == 0)
            {
                _inputOdometerClass = "is-invalid";

                _showValidationOdometer = true;
            }
            if (_breakdownViewModel.OdometerStart > 0)
            {
                _inputOdometerClass = "is-valid";

                _showValidationOdometer = false;
            }
        }


        public async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    //await sourceStream.CopyToAsync(localFileStream);

                    // Read the file into a byte array
                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await sourceStream.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    // Convert the byte array to a base64 string
                    string base64Image = Convert.ToBase64String(fileBytes);

                    _base64List.Add(base64Image);
                }
            }
        }

        private async Task RemovePhoto(string base64)
        {
            var dialog = await _dialogServices.ShowDialogAsync<SimpleCustomizedDialog>(new DialogParameters()
            {
                Height = "auto",
                Title = $"Confirmar ação",
                PreventDismissOnOverlayClick = true,
                PreventScroll = true,
                Modal = true,
            });

            var result = await dialog.Result;

            if (!result.Cancelled)
                _base64List.Remove(base64);
        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            try
            {
                _content = new MultipartFormDataContent();

                _browserFiles.AddFiles(e.GetMultipleFiles());

                string base64File = string.Empty;

                string _fileName = string.Empty;

                foreach (var file in _browserFiles.Multiparts)
                {
                    _content.Add(file);

                    var contentDisposition = file.Headers.GetValues("Content-Disposition").FirstOrDefault();

                    if (!string.IsNullOrEmpty(contentDisposition))
                        _fileName = contentDisposition.Split(';')[2].Replace("filename=", string.Empty).Trim();

                    var bytes = await file.ReadAsByteArrayAsync();

                    base64File = Convert.ToBase64String(bytes);

                    _base64List.Add(base64File);
                }
            }
            catch (ArgumentException arg)
            {
                await _dialogServices.ShowErrorAsync(arg.Message, "Atenção");
                return;
            }
            catch (IOException io)
            {
                await _dialogServices.ShowErrorAsync(io.Message, "Atenção");
                return;
            }
            catch (Exception)
            {
                await _dialogServices.ShowErrorAsync(
                    "Ocorreu um erro inesperado. Caso persistir, favor entrar em contato com o administrador do sistema",
                    "Atenção");
                throw;
            }
            finally
            {
                _browserFiles = new BrowserFiles();
            }
        }

        private async Task ShowErrorAsync(string message, string? title = null) => await _dialogServices.ShowErrorAsync(message, title);
    }
}
