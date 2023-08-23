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
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBillRepository _billRepository;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;

        public TicketController(ITicketRepository ticketRepository, IMapper mapper,
            ISeatRepository seatRepository, IBillRepository billRepository,
            IShowTimeRepository showTimeRepository, MyDbContext context) 
        {
            _ticketRepository = ticketRepository;
            _seatRepository = seatRepository;
            _billRepository = billRepository;
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTickets(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _ticketRepository.GetTickets(page, pageSize != 0 ? pageSize : 10);
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

        //[HttpPost]
        //public IActionResult CreateTicket( [FromBody] List<TicketDto> ticketCreate)
        //{
        //    if (ticketCreate == null)
        //        return BadRequest();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    foreach (var ticket in ticketCreate)
        //    {
        //        var ticketMap = _mapper.Map<Ticket>(ticket);
        //        ticketMap.Bill = _billRepository.GetBillToCheck(ticket.BillId);
        //        if (ticket.SeatId != null)
        //        {
        //            ticketMap.Seat = _seatRepository.GetSeatToCheck(ticket.SeatId);
        //        }
        //        if (ticket.ShowTimeId != null)
        //        {
        //            ticketMap.ShowTime = _showTimeRepository.GetShowTimeToCheck(ticket.ShowTimeId);
        //        }

        //        var tickets = _context.Tickets.Include(item => item.Seat).Include(item => item.ShowTime)
        //            .Where(item => item.ShowTime.Id == ticket.ShowTimeId && item.Seat.Id == ticket.SeatId);
        //        if (tickets.Any())
        //        {
        //            ModelState.AddModelError("", "Ticket already exists");
        //            return BadRequest(ModelState);
        //        }

        //        var price = _context.Tickets
        //        .Include(item => item.Seat)
        //        .Where(item => item.Seat.Id == ticket.SeatId)
        //        .Select(f => f.Seat.Price)
        //        .FirstOrDefault();

        //        var bill = _context.Bills.FirstOrDefault(item => item.Id == ticket.BillId);

        //        if (bill != null)
        //        {
        //            bill.Quantity = _context.Tickets.Count(item => item.Bill.Id == bill.Id);
        //            bill.PriceTotal = bill.Quantity * price;
        //        }

        //        if (!_ticketRepository.CreateTicket(ticketMap))
        //        {
        //            return BadRequest("Error");
        //        }
        //        _billRepository.Save();
        //    }
        //    return Ok("Successfully");
        //}
    }
}
