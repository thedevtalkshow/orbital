using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using orbital.web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add HttpClient for browser navigation
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//// Add HttpClient for orbital.api communication
//builder.Services.AddScoped(sp => 
//{
//    // In development, API typically runs at a specific port
//    // In production, this would be configured differently (e.g., from configuration)
//    var apiBaseAddress = builder.HostEnvironment.IsDevelopment()
//        ? new Uri("https://orbital-api") // Default Aspire development port for the API
//        : new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://api.orbital.com");

//    return new HttpClient { BaseAddress = apiBaseAddress };
//}, serviceKey: "OrbitalApi");


// Alternative approach using named HttpClient with typed client
// builder.Services.AddHttpClient("OrbitalApi", client =>
// {
//     client.BaseAddress = new Uri(builder.HostEnvironment.IsDevelopment()
//         ? "https://localhost:7154"
//         : builder.Configuration["ApiBaseAddress"] ?? "https://api.orbital.com");
// });

await builder.Build().RunAsync();
