using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using OrderTicketFilm.Repository;
using System.Net.Sockets;

namespace OrderTicketFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BillController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly MyDbContext _context;

        public BillController(IBillRepository billRepository, IMapper mapper,
            ICustomerRepository customerRepository, IUserRepository userRepository,
            ITicketRepository ticketRepository, MyDbContext context) 
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBills(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _billRepository.GetBills(page, pageSize != 0 ? pageSize : 10);
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

        [HttpGet("{billId}/tickets")]
        public IActionResult GetTicketsByABill(int billId, int page = 0, int pageSize = 10)
        {
            var bill = _billRepository.GetTicketsByABill(billId, page, pageSize != 0 ? pageSize : 10);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(bill);
        }

        [HttpPost]
        public IActionResult CreateBill([FromBody] BillBuilderDTO billBuilderDTO)
        {
            if (billBuilderDTO == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var itemTicketBuilder in billBuilderDTO.TicketBuilders)
            {
                var tickets = _context.Tickets.Include(item => item.Seat).Include(item => item.ShowTime)
                    .Where(item => item.ShowTime.Id == itemTicketBuilder.ShowTimeId && item.Seat.Id == itemTicketBuilder.SeatId)
                    .FirstOrDefault();
                if (tickets != null)
                {
                    ModelState.AddModelError("", "Ticket already exists");
                    return BadRequest(ModelState);
                }
            }

            var billMap = _mapper.Map<Bill>(billBuilderDTO);
            billMap.Customer = _customerRepository.GetCustomerToCheck(billBuilderDTO.CustomerId);
            billMap.User = _userRepository.GetUserToCheck(billBuilderDTO.UserId);

            var ticketsToAdd = new List<Ticket>(); 

            foreach (var itemTicketBuilder in billBuilderDTO.TicketBuilders)
            {
                var seat = _context.Seats.Find(itemTicketBuilder.SeatId);
                var showTime = _context.ShowTimes.Find(itemTicketBuilder.ShowTimeId);

                if (seat != null && showTime != null)
                {
                    var newTicket = new Ticket
                    {
                        Seat = seat,
                        ShowTime = showTime,
                    };

                    ticketsToAdd.Add(newTicket);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid seat or show time ID");
                    return BadRequest(ModelState);
                }
            }

            billMap.Tickets = ticketsToAdd;

            var newBill = _billRepository.CreateBill(billMap);
            if (!newBill)
            {
                return BadRequest("Error creating bill");
            }

            else return Ok("Successfully");
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
