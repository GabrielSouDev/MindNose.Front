using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MindNose.Front;
using MindNose.Front.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

//builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri( "http://localhost:5000") });

builder.Services.AddScoped<MindNoseService>();
builder.Services.AddSingleton<CytoscapeService>();

await builder.Build().RunAsync();