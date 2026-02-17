using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

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
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = _dbContext.Customers.ToList();
            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _dbContext.Customers.FirstOrDefault(c => c.Id == id);
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
                IsActive = true,
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