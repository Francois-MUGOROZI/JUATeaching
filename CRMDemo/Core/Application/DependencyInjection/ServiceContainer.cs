using Application.Services.Customers;
using Application.Services.Reports;
using Application.Services.Tickets;
using Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Domain services (business logic) 
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IIdentityService, IdentityService>();

            // Report services
            services.AddScoped<ICustomerReportService, CustomerReportService>();

            return services;
        }
    }
}
