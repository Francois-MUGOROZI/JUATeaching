using Web.Components;
using Application.Services.Customers;
using Application.Services.Users;
using Infrastructure.DependencyInjection;

using MudBlazor.Services;
using Infrastructure.Identity;

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
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();


var app = builder.Build();

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
app.MapStaticAssets(); // For wwwroot assests
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
