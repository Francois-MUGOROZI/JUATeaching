
using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomer _customer;
        private readonly ILogger<CustomerService> _logger;

        // constructor 
        public CustomerService(ICustomer customer, ILogger<CustomerService> logger)
        {
            _customer = customer;
            _logger = logger;
        }

        public Customer GetCustomerById(int id)
        {
            return _customer.GetCustomerById(id);
        }
        public List<Customer> GetAllCustomers(CustomerFilterDTO filter)
        {
            List<Customer> customers = _customer.GetAllCustomers(filter);
            return customers;
        }
        public void CreateCustomer(CustomerCreateDTO customerDTO)
        {
            // check if exist and NID exist 
            _customer.CreateCustomer(customerDTO);
            _logger.LogInformation("Customer created: {Email} {FirstName} {LastName}", customerDTO.Email, customerDTO.FirstName, customerDTO.LastName);
        }

        public void UpdateCustomer(int id, CustomerUpdateDTO customerDTO)
        {
            _customer.UpdateCustomer(id, customerDTO);
            _logger.LogInformation("Customer updated: {CustomerId}", id);
        }
    }
}