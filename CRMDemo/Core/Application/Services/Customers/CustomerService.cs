
using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomer _customer;

        // constructor 
        public CustomerService(ICustomer customer)
        {
            _customer = customer;
        }

        public Customer GetCustomerById(int id)
        {
            return _customer.GetCustomerById(id);
        }
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = _customer.GetAllCustomers();
            return customers;
        }
        public void CreateCustomer(CustomerCreateDTO customerDTO)
        {
            // check if exist and NID exist 
            _customer.CreateCustomer(customerDTO);
        }

        public void UpdateCustomer(int id, CustomerUpdateDTO customerDTO)
        {
            _customer.UpdateCustomer(id, customerDTO);
        }
    }
}