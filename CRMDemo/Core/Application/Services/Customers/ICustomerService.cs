using Application.DTO;
using Domain.Entities;

namespace Application.Services.Customers
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int id);
        List<Customer> GetAllCustomers();
        void CreateCustomer(CustomerCreateDTO customerDTO);
        void UpdateCustomer(int id, CustomerUpdateDTO customerDTO);
    }
}