using BlazorRPG;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap;

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

var host = builder.Build();
host.Services.UseBootstrapProviders();
await host.RunAsync();

/*
 * Add using references to Blazorize,
 * set up Blazorise services
 * Configure Blazorise Bootstrap provider
 */