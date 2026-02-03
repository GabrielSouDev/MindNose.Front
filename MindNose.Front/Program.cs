using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MindNose.Front;
using MindNose.Front.Models.Consts;
using MindNose.Front.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LocalStorageAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<LocalStorageAuthProvider>());
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();
builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy("UserOrAdmin", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole(Role.User) || context.User.IsInRole(Role.Admin)));
});
builder.Services.AddRadzenComponents();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://api.mindnose.cloud/");
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<LocalStorageTheme>();
builder.Services.AddScoped<MindNoseApiService>();
builder.Services.AddSingleton<CytoscapeService>();
builder.Services.AddSingleton<OpenRouterService>();

var app = builder.Build();

var mindnoseservice = app.Services.GetService<MindNoseApiService>()!;

await mindnoseservice.InitializeAsync();

await app.RunAsync();