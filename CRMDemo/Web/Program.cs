using Web.Components;
using Web.Helpers;
using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using Infrastructure.Data;

using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices(); // MudBlazor services

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add authentication state provider for Blazor
builder.Services.AddCascadingAuthenticationState();

// Add controllers for account endpoints (login/logout)
builder.Services.AddControllers();

// Dependency Injection for Infrastructure Layer
builder.Services.AddInfrastructureService(builder.Configuration);

// Dependency Injection for Application Layer
builder.Services.AddApplicationServices();

// API Client - single reusable typed HttpClient for calling the API project
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiSettings:ApiKey"]!);
});


var app = builder.Build();

// Seed database on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add auth
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers(); // Account login/logout endpoints
app.MapStaticAssets(); // For wwwroot assets
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
