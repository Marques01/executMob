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
                    fonts.AddFont("Comfortaa-VariableFont_wght.ttf", "Comfortaa");
                });

            builder.Services.AddFluentUIComponents(options =>
            {
                options.HostingModel = BlazorHostingModel.Hybrid;
                options.UseTooltipServiceProvider = true;
            });

            //builder.Configuration.AddJsonFile("appsettings.json").Build();
            //string uri = builder.Configuration.GetSection("Services:URL").Value!;
            string uri = "http://179.66.149.106:5000/";
            DependecyInjection.ApplyConfigurations(builder.Services, uri);

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
