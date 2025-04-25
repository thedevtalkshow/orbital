using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using orbital.web;
using orbital.web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var MeetingsEndpoint = builder.Configuration["MeetingsEndpoint"];

builder.Services.AddLogging();

// Add HttpClient for browser navigation
builder.Services.AddSingleton<HttpClient>(hc => new HttpClient() { BaseAddress = new Uri("https://localhost:7024") });

builder.Services.AddScoped<MeetingsService>();

await builder.Build().RunAsync();
