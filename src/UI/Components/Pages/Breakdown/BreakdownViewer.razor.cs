using Microsoft.AspNetCore.Components;

namespace UI.Components.Pages.Breakdown;

public partial class BreakdownViewer
{
    [Parameter]
    public string Id { get; set; } = string.Empty;

    private bool
        _isLoading = false;

    private Domain.Entities.Breakdown _breakdown = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        await GetBreakdownAsync();

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

    private async Task OpenImageViewer(string base64)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BreakdownVisualizerImage(base64));
    }
}