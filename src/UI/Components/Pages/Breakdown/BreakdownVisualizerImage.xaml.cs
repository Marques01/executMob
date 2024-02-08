namespace UI.Components.Pages.Breakdown;

public partial class BreakdownVisualizerImage : ContentPage
{
    public BreakdownVisualizerImage(string base64)
    {
        InitializeComponent();
        CarregarImagemBase64(base64);
    }
    private void CarregarImagemBase64(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        Stream stream = new MemoryStream(bytes);
        ImagemTelaCheia.Source = ImageSource.FromStream(() => stream);
    }
}