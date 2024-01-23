using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using UI.Configuration;

namespace UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddFluentUIComponents(options =>
            {
                options.HostingModel = BlazorHostingModel.Hybrid;
                options.UseTooltipServiceProvider = true;
            });
            DependecyInjection.ApplyConfigurations(builder.Services);

            builder.Services.AddOptions();

            builder.Services.AddAuthorizationCore();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
