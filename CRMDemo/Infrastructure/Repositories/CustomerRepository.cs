using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        // Retrieving customers
        public List<Customer> GetAllCustomers(CustomerFilterDTO filter)
        {
            IQueryable<Customer> query = _dbContext.Customers;
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(c => c.FirstName
                .Contains(filter.SearchTerm) || c.LastName.Contains(filter.SearchTerm) || c.Email.Contains(filter.SearchTerm));
            }
            if (filter.Status.HasValue)
            {
                query = query.Where(c => c.Status == filter.Status.Value);
            }
            List<Customer> customers = query.ToList();
            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _dbContext.Customers.Include(a => a.Tickets).FirstOrDefault(c => c.Id == id);
        }

        public void CreateCustomer(CustomerCreateDTO customerDTO)
        {
            Customer customer = new()
            {
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                Email = customerDTO.Email,
                PhoneNumber = customerDTO.PhoneNumber,
                CreatedAt = DateTime.Now,
                Status = CustomerStatus.Active,
                CreatedById = 1
            };
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
        }

        public void UpdateCustomer(int id, CustomerUpdateDTO customerDTO)
        {
            var customer = _dbContext.Customers.Find(id);
            if (customer == null) return;

            customer.FirstName = customerDTO.FirstName;
            customer.LastName = customerDTO.LastName;
            customer.PhoneNumber = customerDTO.PhoneNumber;
            _dbContext.SaveChanges();
        }
    }
}