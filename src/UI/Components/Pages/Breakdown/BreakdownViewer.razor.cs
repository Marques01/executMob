using Domain.Entities;
using Domain.Entities.Response;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using Domain.Models;
using UI.ViewModels;

namespace UI.Components.Pages.Breakdown;

public partial class BreakdownViewer
{
    [Parameter]
    public string Id { get; set; } = string.Empty;

    private bool
        _isLoading = false,
        _formIsValid = false;

    private string
        _loadingMessage = string.Empty;

    private Domain.Entities.Breakdown _breakdown = new();

    private ProductViewModel _productViewModel = new();

    private IEnumerable<Product> _products = Enumerable.Empty<Product>();

    private List<ProductViewModel> _productsList = new();

    private List<PictureViewModel> _pictureViewModelList = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        _loadingMessage = "Aguarde um momento estamos enquanto carregando as informações...";

        await GetBreakdownAsync();

        await GetProductsAsync();

        _isLoading = false;
    }
    private async Task GetBreakdownAsync()
    {
        try
        {
            int id = int.Parse(Id);
            _breakdown = await _breakdownServices.GetBreakdownAsync(id);
        }
        catch (ArgumentException arg)
        {
            await _dialogService.ShowWarningAsync(arg.Message, "Atenção");
            return;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "Erro");
            return;
        }
    }

    private async Task GetProductsAsync()
    {
        try
        {
            _products = await _productServices.GetProductsAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "Erro");
        }
    }

    private async Task OpenImageViewer(string base64)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BreakdownVisualizerImage(base64));
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
    private void RemovePhoto(Guid id)
    {
        PictureViewModel pictureViewModel = _pictureViewModelList.First(x => x.Id == id);
        _pictureViewModelList.Remove(pictureViewModel);
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

    private async Task<OrderServiceProductResponseModel> AddProductAsync(
        OrderServiceProductCostumerModel orderServiceProduct)
    {
        var productResponseModel = await _orderServiceProcessing.AddProductAsync(orderServiceProduct);

        return productResponseModel;
    }

    private async Task<BreakdownResponseModel> UpdateAsync(BreakdownCostumerUpdateModel breakdownCostumerModel)
    {
        var breakdownResponseModel = await _breakdownServices.UpdateAsync(breakdownCostumerModel);

        return breakdownResponseModel;
    }

    private async Task CloseOrderServiceAsync(int orderServiceId)
    {
        await _orderServiceProcessing.CloseOrderServiceAsync(orderServiceId);
    }

    private async Task HandleSubmitAsync()
    {
        try
        {
            _isLoading = true;
            _loadingMessage = "Aguarde um momento estamos finalizando a ordem de serviço...";
            StateHasChanged();

            await Task.Delay(500);

            if (_breakdown.OdometerEnd < _breakdown.OdometerStart)
                throw new ArgumentException("O odômetro final não pode ser menor que o odômetro inicial.");

            if (_pictureViewModelList.Count < 1)
                throw new ArgumentException(
                    "Caro colaborador, lembre-se de anexar ao menos uma imagem do material utilizado. Agradecemos sua cooperação.");

            if (_productsList.Count == 0)
                throw new ArgumentException(
                    "Caro colaborador, lembramos que é obrigatório o cadastramento de ao menos um material como parte do processo. Agradecemos sua cooperação.");

            foreach (var picture in _pictureViewModelList)
            {
                var pictureSaveResponseModel = await UploadPictureAsync(picture);

                await SaveBreakdownImage(int.Parse(Id), pictureSaveResponseModel.Model!.Name);
            }

            foreach (var product in _productsList)
            {
                OrderServiceProductCostumerModel productCostumerModel = new()
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,                    
                    OrderServiceId = _breakdown.OrderService!.OrderServiceId
                };

                await AddProductAsync(productCostumerModel);
            }

            BreakdownCostumerUpdateModel breakdownCostumerModel = new()
            {
                BreakdownId = _breakdown.BreakdownId,
                OdometerEnd = _breakdown.OdometerEnd,
            };

            await UpdateAsync(breakdownCostumerModel);

            await CloseOrderServiceAsync(_breakdown.OrderService!.OrderServiceId);

            var dialogReference =
                await _dialogService.ShowSuccessAsync("Ordem de serviço finalizada com sucesso.", "Sucesso");

            var result = await dialogReference.Result;

            if (!result.Cancelled)
                _navigationManager.NavigateTo("/breakdowns", true);
        }
        catch (ArgumentException arg)
        {
            await _dialogService.ShowWarningAsync(arg.Message, "Atenção");
        }
        catch (Exception)
        {
            await _dialogService.ShowErrorAsync("Ocorreu um erro inesperado, favor tente novamente mais tarde.",
                "Erro");
        }
        finally
        {
            _isLoading = false;
        }
    }

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