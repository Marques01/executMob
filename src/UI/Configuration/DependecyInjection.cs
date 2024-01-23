using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace UI.Configuration
{
    public class DependecyInjection
    {
        public static void ApplyConfigurations(IServiceCollection services)
        {
            services.AddScoped<TokenAuthenticationProvider>();

            services.AddScoped<IAuthorizeServices, TokenAuthenticationProvider>(
                provider => provider.GetRequiredService<TokenAuthenticationProvider>()
                );

            services.AddScoped<AuthenticationStateProvider, TokenAuthenticationProvider>(
              provider => provider.GetRequiredService<TokenAuthenticationProvider>());

            services.AddHttpClient<TokenAuthenticationProvider>(x =>
            {
                x.BaseAddress = new Uri("https://localhost:7256/");
                x.DefaultRequestHeaders.Add("Accept", "application/+json");
            });

            services.AddHttpClient<IAccountServices, AccountServices>(x =>
            {
                x.BaseAddress = new Uri("https://localhost:7256/");
                x.DefaultRequestHeaders.Add("Accept", "application/+json");
            });
        }
    }
}
