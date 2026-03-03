using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore; // ORM
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Identity;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add infrastructure services here, e.g., DbContext, Repositories, etc.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CRMDemoMSSQLConnection")), ServiceLifetime.Scoped
            );

            // Register identity service
            services.AddAuthenticationService(configuration);

            // Register User Context (for accessing current user anywhere)
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            // Register Repositories 
            services.AddScoped<ICustomer, CustomerRepository>();
            services.AddScoped<ITicket, TicketRepository>();
            services.AddScoped<IIdentity, IdentityRepository>();

            // Register Data Seeder
            services.AddScoped<IDataSeeder, DataSeeder>();

            return services;
        }
    }
}