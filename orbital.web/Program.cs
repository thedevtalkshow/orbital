using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using orbital.web;
using orbital.web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var MeetingsEndpoint = builder.Configuration["MeetingsEndpoint"] ?? "https://localhost:7024";

builder.Services.AddLogging();

builder.Services.AddHttpClient<IMeetingsService, MeetingsHttpClient>(client =>
{
    client.BaseAddress = new Uri(MeetingsEndpoint);
});

await builder.Build().RunAsync();
