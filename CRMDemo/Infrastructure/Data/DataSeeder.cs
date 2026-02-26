using Bogus;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }

    public class DataSeeder : IDataSeeder
    {
        private readonly ApplicationDbContext _context;
        private const int MinCustomerCount = 40;
        private const int MinTicketsPerCustomer = 10;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Check if seeding is needed
            var customerCount = await _context.Customers.CountAsync();
            var ticketCount = await _context.Tickets.CountAsync();

            // Skip seeding if database already has enough data
            if (customerCount >= MinCustomerCount && ticketCount >= MinCustomerCount * MinTicketsPerCustomer)
            {
                return;
            }

            // Generate and seed customers with tickets
            var customers = GenerateCustomers(MinCustomerCount);

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();
        }

        private List<Customer> GenerateCustomers(int count)
        {
            var customerFaker = new Faker<Customer>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Status, f => f.PickRandom<CustomerStatus>())
                .RuleFor(c => c.CreatedAt, f => DateTime.Now)
                .RuleFor(c => c.UpdatedAt, f => DateTime.Now)
                .RuleFor(c => c.CreatedById, f => null)
                .RuleFor(c => c.UpdatedById, f => null)
                .RuleFor(c => c.Tickets, (f, c) => GenerateTickets(MinTicketsPerCustomer));

            return customerFaker.Generate(count);
        }

        private List<Ticket> GenerateTickets(int count)
        {
            var ticketFaker = new Faker<Ticket>()
                .RuleFor(t => t.Subject, f => f.Lorem.Sentence(3, 5))
                .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                .RuleFor(t => t.Status, f => f.PickRandom<TicketStatus>())
                .RuleFor(t => t.CreatedAt, f => DateTime.Now)
                .RuleFor(t => t.UpdatedAt, f => DateTime.Now)
                .RuleFor(t => t.CreatedById, f => null)
                .RuleFor(t => t.UpdatedById, f => null);

            return ticketFaker.Generate(count);
        }
    }
}
