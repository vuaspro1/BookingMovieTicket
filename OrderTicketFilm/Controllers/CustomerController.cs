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
        private readonly MyDbContext _context;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper,
            MyDbContext context)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCustomers(int page = 0, int pageSize = 10)
        {
            try
            {
                //var result = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());
                //return Ok(result);
                var result = _customerRepository.GetCustomers(page, pageSize != 0 ? pageSize : 10);
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

        [HttpGet("{search}/customers")]
        public IActionResult GetCustomerBySearch(string search, int page = 0, int pageSize = 10)
        {
            var customer = _customerRepository.GetCustomerBySearch(search, page, pageSize != 0 ? pageSize : 10);
            if (customer == null )
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);
        }

        [HttpGet("{id}/bills")]
        public IActionResult GetBillsByACustomer(int id, int page = 0, int pageSize = 10)
        {
            if (!_customerRepository.CustomerExists(id))
                return NotFound();

            var bills = _customerRepository.GetBillByACustomer(id, page, pageSize != 0 ? pageSize : 10);

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
                ModelState.AddModelError("","Phone already exists");
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

            if (!ModelState.IsValid) return BadRequest();

            var existingCustomer = _context.Customers.FirstOrDefault(item => item.Id == id);
            if (existingCustomer == null) 
                return NotFound();

            _mapper.Map(customerUpdate, existingCustomer);

            if (!_customerRepository.UpdateCustomer(existingCustomer))
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
