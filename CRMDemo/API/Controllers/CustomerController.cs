using Microsoft.AspNetCore.Mvc;

using Application.Services.Customers;
using Domain.Entities;
using Application.DTO;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{

    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("/api/customers")]
    public IEnumerable<Customer> GetCustomers()
    {
        return _customerService.GetAllCustomers(new CustomerFilterDTO() { });
    }

    [HttpGet("/api/customers/{id}")]
    public Customer GetCustomerById(int id)
    {
        var customer = _customerService.GetCustomerById(id);
        var result = new Customer
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt
        };
        return result;
    }

    [HttpPost("/api/customers")]
    public IActionResult CreateCustomer([FromBody] CustomerCreateDTO customerDTO)
    {
        _customerService.CreateCustomer(customerDTO);
        return Ok();
    }
}
