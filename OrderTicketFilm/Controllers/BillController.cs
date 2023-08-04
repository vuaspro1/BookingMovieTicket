using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using OrderTicketFilm.Repository;

namespace OrderTicketFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public BillController(IBillRepository billRepository, IMapper mapper,
            ICustomerRepository customerRepository, IUserRepository userRepository) 
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetBills(int page)
        {
            try
            {
                var result = _billRepository.GetBills(page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the bill.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBill(int id)
        {
            if (!_billRepository.BillExists(id))
                return NotFound();

            var bill = _billRepository.GetBill(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bill);
        }

        [HttpGet("getTicketsByABill")]
        public IActionResult GetTicketsByABill(int billId, int page)
        {
            var bill = _billRepository.GetTicketsByABill(billId, page);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(bill);
        }

        [HttpPost]
        public IActionResult CreateBill([FromBody] BillDto billCreate)
        {
            if (billCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var billMap = _mapper.Map<Bill>(billCreate);
            billMap.Customer = _customerRepository.GetCustomerToCheck(billCreate.CustomerId);
            billMap.User = _userRepository.GetUserToCheck(billCreate.UserId);

            if (!_billRepository.CreateBill(billMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBill(int id, [FromBody] BillDto billUpdate)
        {
            if (billUpdate == null)
                return BadRequest(ModelState);

            if (id != billUpdate.Id)
                return BadRequest(ModelState);

            if (!_billRepository.BillExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var billMap = _mapper.Map<Bill>(billUpdate);
            billMap.Customer = _customerRepository.GetCustomerToCheck(billUpdate.CustomerId);
            billMap.User = _userRepository.GetUserToCheck(billUpdate.UserId);

            if (!_billRepository.UpdateBill(billMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            if (!_billRepository.BillExists(id))
            {
                return NotFound();
            }

            _billRepository.DeleteBill(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
