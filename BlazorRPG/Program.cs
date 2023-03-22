using BlazorRPG;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap;
using RPG.Game.Engine.ViewModels;

 var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
              .AddBlazorise(options =>
              {
                  options.ChangeTextOnKeyPress = false;
              })
              .AddBootstrapProviders();

// add app-specific/custom services and view models here...
AppServiceConfig.ConfigureAppServices(builder.Services);

var host = builder.Build();
host.Services.UseBootstrapProviders();

// initialize app-specific/custom services and view models here...
AppServiceConfig.InitializeAppServices(host.Services);

await host.RunAsync();




/*
 * Add using references to Blazorize,
 * set up Blazorise services
 * Configure Blazorise Bootstrap provider
 */