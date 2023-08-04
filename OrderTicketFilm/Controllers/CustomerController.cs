using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using OrderTicketFilm.Repository;

namespace OrderTicketFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCustomers(int page)
        {
            try
            {
                //var result = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());
                //return Ok(result);
                var result = _customerRepository.GetCustomers(page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the customer.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            if (!_customerRepository.CustomerExists(id))
                return NotFound();

            var customer = _customerRepository.GetCustomer(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);
        }

        [HttpGet("getCustomerByName")]
        public IActionResult GetCustomerByName(string? name, int page)
        {
            var customer = _customerRepository.GetCustomerByName(name, page);
            if (!customer.Any())
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);
        }

        [HttpGet("getBillsByACustomer")]
        public IActionResult GetBillsByACustomer(int id, int page)
        {
            if (!_customerRepository.CustomerExists(id))
                return NotFound();

            var bills = _customerRepository.GetBillByACustomer(id, page);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bills);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
        {
            if (customerCreate == null)
                return BadRequest();
            var customer = _customerRepository.GetCustomersToCheck()
                .Where(item => item.Phone.Trim() == customerCreate.Phone.TrimEnd())
                .FirstOrDefault();
            if (customer != null)
            {
                ModelState.AddModelError("","Customer already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerMap = _mapper.Map<Customer>(customerCreate);

            if (!_customerRepository.CreateCustomer(customerMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id,[FromBody] CustomerDto customerUpdate)
        {
            if (customerUpdate == null)
                return BadRequest();
            if (id != customerUpdate.Id)
                return BadRequest();
            if (!_customerRepository.CustomerExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var customerMap = _mapper.Map<Customer>(customerUpdate);

            if (!_customerRepository.UpdateCustomer(customerMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer([FromRoute]int id)
        {
            if (!_customerRepository.CustomerExists(id))
            {
                return NotFound();
            }

            _customerRepository.DeleteCustomer(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
