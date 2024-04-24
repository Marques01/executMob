using Domain.Entities;
using Domain.Entities.Response;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Fast.Components.FluentUI;
using System.Net.Http.Headers;
using Domain.Enum;
using UI.Components.Layout;
using UI.ViewModels;

namespace UI.Components.Pages.Breakdown
{
    public partial class Create
    {
        private bool
            _isLoading = false,
            _showValidationOdometer = false,
            _showValidationEmployee = false,
            _showValidationDescription = false,
            _showValidationVehicle = false,
            _formIsValid = false;

        private string
            _inputOdometerClass = string.Empty,
            _inputDescriptionClass = string.Empty,
            _inputVehicleClass = string.Empty,
            _inputEmployeeClass = string.Empty;

        private BreakdownViewModel _breakdownViewModel = new();

        private List<Vehicle> _vehicles = new();

        private List<User> _users = new();

        private List<User> _usersList = new();

        private List<PictureViewModel> _pictureViewModelList = new();

        private EditContext? _editContext;

        private ProductViewModel _productViewModel = new();

        private IEnumerable<Product> _products = Enumerable.Empty<Product>();

        private List<ProductViewModel> _productsList = new();

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_breakdownViewModel);

            _isLoading = true;

            await GetVehiclesAsync();

            await GetUsersAsync();

            await GetProductsAsync();

            _isLoading = false;
        }

        private async Task GetVehiclesAsync()
        {
            try
            {
                var vehicles = await _vehicleServices.GetVehiclesAsync();

                _vehicles = vehicles.ToList();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message, "Atenção");
                return;
            }
        }

        private async Task GetUsersAsync()
        {
            try
            {
                var users = await _userServices.GetUsersAsync();

                _users = users.ToList();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message, "Atenção");
                return;
            }
        }

        private async Task<PictureSaveResponseModel> UploadPictureAsync(PictureViewModel picture)
        {
            using var content = new MultipartFormDataContent();
            using var byteContent = new ByteArrayContent(picture.Bytes);
            using var stream = await byteContent.ReadAsStreamAsync();
            using var streamContent = new StreamContent(stream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(streamContent, "\"files\"", picture.FileName);

            var pictureSaveResponseModel = await _pictureStorageServices.UploadAsync(content);

            return pictureSaveResponseModel;
        }

        private async Task<Domain.Entities.Breakdown> CreateBreakDownAsync()
        {
            BreakdownCostumerModel breakdownCostumerModel = new()
            {
                Description = _breakdownViewModel.Description,
                OdometerStart = _breakdownViewModel.OdometerStart,
                VehicleId = _breakdownViewModel.VehicleId,
            };

            var breakDownResponseModel = await _breakdownServices.CreateAsync(breakdownCostumerModel);

            return breakDownResponseModel.Model!;
        }

        private async Task<OrderServiceResponseModel> CreateOrderServiceAsync(Domain.Entities.Breakdown breakdown)
        {
            OrderServiceCostumerModel orderServiceCostumerModel = new()
            {
                BreakdownId = breakdown.BreakdownId,
                Description = _breakdownViewModel.Description,
                Status = StatusEnum.Opened,
            };

            var orderServiceResponseModel = await _orderServiceProcessing.CreateAsync(orderServiceCostumerModel);

            return orderServiceResponseModel;
        }

        private async Task<BreakdownImagesResponseModel> SaveBreakdownImage(int breakdownId, string fileName)
        {
            BreakdownImages breakdownImages = new()
            {
                BreakdownId = breakdownId,
                Image = fileName
            };

            var pictureSaveResponseModel = await _breakdownServices.SaveDirectoryImage(breakdownImages);

            return pictureSaveResponseModel;
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

            ValidationEmployeeMessage();
        }
        private async Task GetProductsAsync()
        {
            try
            {
                _products = await _productServices.GetProductsAsync();
            }
            catch (Exception ex)
            {
                await _dialogServices.ShowErrorAsync(ex.Message, "Erro");
            }
        }

        private async Task HandleSubmit()
        {
            try
            {
                ValidationAllFields();

                if (_pictureViewModelList.Count < 2)
                    throw new ArgumentException("É de extrema importância seguir as orientações de envio. Não se esqueça de anexar imagens dos materiais, juntamente com a captura de uma selfie. Este procedimento é obrigatório para prosseguir.");

                if (_productsList.Count() == 0)
                    throw new ArgumentException("É necessário adicionar ao menos um material para prosseguir.");

                if (_editContext is not null && _editContext.Validate() && _pictureViewModelList.Count >= 2)
                {
                    _isLoading = true;

                    await Task.Delay(500);

                    var breakdown = await CreateBreakDownAsync();

                    foreach (var picture in _pictureViewModelList)
                    {
                        var pictureSaveResponseModel = await UploadPictureAsync(picture);

                        await SaveBreakdownImage(breakdown.BreakdownId, pictureSaveResponseModel.Model!.Name);
                    }

                    foreach (var user in _usersList)
                    {
                        BreakdownUserCostumerModel costumerModel = new()
                        {
                            BreakdownId = breakdown.BreakdownId,
                            UserId = user.UserId
                        };

                        await _breakdownServices.AssociateUser(costumerModel);
                    }

                    var orderServiceResponseModel = await CreateOrderServiceAsync(breakdown);

                    if (orderServiceResponseModel is not null && orderServiceResponseModel.Model is not null)
                    {
                        foreach (var product in _productsList)
                        {
                            OrderServiceProductCostumerModel productCostumerModel = new()
                            {
                                ProductId = product.ProductId,
                                Quantity = product.Quantity,
                                Description = "Material Saída",
                                OrderServiceId = orderServiceResponseModel.Model.OrderServiceId
                            };

                            await AddProductAsync(productCostumerModel);
                        }
                    }

                    var dialogReference = await _dialogServices.ShowSuccessAsync("Avaria cadastrada com sucesso!", "Sucesso");

                    var result = await dialogReference.Result;

                    _isLoading = false;

                    if (!result.Cancelled)
                        RedirectToIndex();
                }

            }
            catch (ArgumentException arg)
            {
                await ShowErrorAsync(arg.Message, "Atenção");
                return;
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message, "Atenção");
                return;
            }
        }

        private async Task<OrderServiceProductResponseModel> AddProductAsync(
            OrderServiceProductCostumerModel orderServiceProduct)
        {
            var productResponseModel = await _orderServiceProcessing.AddProductAsync(orderServiceProduct);

            return productResponseModel;
        }

        public async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo is not null)
                {
                    using Stream sourceStream = await photo.OpenReadAsync();

                    byte[] fileBytes;

                    using (var memoryStream = new MemoryStream())
                    {
                        await sourceStream.CopyToAsync(memoryStream);

                        fileBytes = memoryStream.ToArray();
                    }
                    string base64Image = Convert.ToBase64String(fileBytes);

                    _pictureViewModelList.Add(new()
                    {
                        Content = base64Image,
                        FileName = photo.FileName,
                        Size = fileBytes.Length,
                        Bytes = fileBytes
                    });
                }
            }
        }
        private async Task RemovePhoto(Guid id)
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
            {
                PictureViewModel pictureViewModel = _pictureViewModelList.First(x => x.Id == id);
                _pictureViewModelList.Remove(pictureViewModel);
            }
        }

        private void RedirectToIndex() => _navigationManager.NavigateTo("/", true);

        private void OnVehicleChanged(ChangeEventArgs e)
        {
            ValidationVehicleMessage();
        }

        private void ValidationDescriptionMessage()
        {
            _inputDescriptionClass = _breakdownViewModel.Description.Length > 0 ? "is-valid" : "is-invalid";
            _showValidationDescription = _breakdownViewModel.Description.Length <= 0;
        }

        private void ValidationEmployeeMessage()
        {
            _inputEmployeeClass = _usersList.Count == 2 ? "is-valid" : "is-invalid";
            _showValidationEmployee = _usersList.Count < 2;
        }
        private void ValidationVehicleMessage()
        {
            _inputVehicleClass = _breakdownViewModel.VehicleId > 0 ? "is-valid" : "is-invalid";
            _showValidationVehicle = _breakdownViewModel.VehicleId <= 0;
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

        private void ValidationAllFields()
        {
            ValidationDescriptionMessage();
            ValidationEmployeeMessage();
            ValidationOdotometerMessage();
            ValidationVehicleMessage();
        }

        private async Task ShowErrorAsync(string message, string? title = null) => await _dialogServices.ShowErrorAsync(message, title);

        #region Events

        private void OnMaterialChanged(ChangeEventArgs e)
        {
            string materialId = e.Value!.ToString()!;
            _productViewModel.ProductId = int.Parse(materialId);
            FormIsValid();
        }

        private void OnQuantityChanged(ChangeEventArgs e)
        {
            string quantity = e.Value!.ToString()!;
            _productViewModel.Quantity = int.Parse(quantity);
            FormIsValid();
        }

        private void OnMaterialConfirm()
        {
            if (_formIsValid)
            {
                var product = _products.FirstOrDefault(x => x.ProductId == _productViewModel.ProductId);

                if (product is not null)
                {
                    _productsList.Add(new()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Quantity = _productViewModel.Quantity,
                        ProductId = product.ProductId
                    });
                }
            }

            _productViewModel = new();
            FormIsValid();
        }

        private void OnMaterialRemove(ProductViewModel product)
        {
            _productsList.Remove(product);
        }

        private void FormIsValid() => _formIsValid = _productViewModel.ProductId > 0 && _productViewModel.Quantity > 0;

        #endregion
    }
}
