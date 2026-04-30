using Application.Services.Customers;
using Application.Services.Tickets;
using Application.Services.Users;
using Infrastructure.DependencyInjection;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Dependency Injection for Infrastructure Layer
builder.Services.AddInfrastructureService(builder.Configuration);

// Dependency Injection for Application Layer
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
