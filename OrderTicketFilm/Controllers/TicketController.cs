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
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBillRepository _billRepository;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;

        public TicketController(ITicketRepository ticketRepository, IMapper mapper,
            ISeatRepository seatRepository, IBillRepository billRepository,
            IShowTimeRepository showTimeRepository) 
        {
            _ticketRepository = ticketRepository;
            _seatRepository = seatRepository;
            _billRepository = billRepository;
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTickets()
        {
            try
            {
                var result = _ticketRepository.GetTickets();
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the ticket.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTicket(int id)
        {
            if (!_ticketRepository.TicketExists(id))
                return NotFound();

            var ticket = _ticketRepository.GetTicket(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ticket);
        }

        [HttpPost]
        public IActionResult CreateTicket([FromQuery] int billId, [FromQuery] int seatId,
            [FromQuery] int showTimeId, [FromBody] TicketDto ticketCreate)
        {
            if (ticketCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticketMap = _mapper.Map<Ticket>(ticketCreate);
            ticketMap.Bill = _billRepository.GetBillToCheck(billId);
            ticketMap.Seat = _seatRepository.GetSeatToCheck(seatId);
            ticketMap.ShowTime = _showTimeRepository.GetShowTimeToCheck(showTimeId);

            if (!_ticketRepository.CreateTicket(ticketMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, [FromBody] TicketDto ticketUpdate)
        {
            if (ticketUpdate == null)
                return BadRequest(ModelState);

            if (id != ticketUpdate.Id)
                return BadRequest(ModelState);

            if (!_ticketRepository.TicketExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ticketMap = _mapper.Map<Ticket>(ticketUpdate);

            if (!_ticketRepository.UpdateTicket(ticketMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            if (!_ticketRepository.TicketExists(id))
            {
                return NotFound();
            }

            _ticketRepository.DeleteTicket(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
