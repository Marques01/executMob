﻿using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace UI.Configuration
{
    public class DependecyInjection
    {
        public static void ApplyConfigurations(IServiceCollection services, string uri)
        {
            services.AddScoped<TokenAuthenticationProvider>();

            services.AddScoped<IAuthorizeServices, TokenAuthenticationProvider>(
                provider => provider.GetRequiredService<TokenAuthenticationProvider>()
                );

            services.AddScoped<AuthenticationStateProvider, TokenAuthenticationProvider>(
              provider => provider.GetRequiredService<TokenAuthenticationProvider>());

            services.AddHttpClient<TokenAuthenticationProvider>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IAccountServices, AccountServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IBreakdownServices, BreakdownServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IVehicleServices, VehicleServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IUserServices, UserServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IPictureStorageServices, PictureStorageServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IOrderServiceProcessing, OrderServiceProcessing>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IProductServices, ProductServices>(x =>
            {
                x.BaseAddress = new Uri(uri);
                x.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
