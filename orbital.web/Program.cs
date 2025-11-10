using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using orbital.core.Metadata;
using orbital.web;
using orbital.web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// var MeetingsEndpoint = builder.Configuration["MeetingsEndpoint"]; // ?? "https://localhost:7024";

builder.Services.AddLogging();

var baseAddress = new Uri("https+http://orbital-api");

builder.Services.AddHttpClient<IMeetingsService, MeetingsHttpClient>(client =>
{
    client.BaseAddress = baseAddress;
});

// Register the meeting state service
builder.Services.AddScoped<IMeetingStateService, MeetingStateService>();

// Metadata HttpClient
builder.Services.AddHttpClient<IMetadataService, MetadataHttpClient>(client =>
{
    client.BaseAddress = baseAddress;
});

await builder.Build().RunAsync();
