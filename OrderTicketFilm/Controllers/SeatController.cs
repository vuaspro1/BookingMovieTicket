using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    public class SeatController : Controller
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly ISeatStatusRepository _seatStatus;
        private readonly MyDbContext _context;

        public SeatController(ISeatRepository seatRepository, IMapper mapper, 
            IRoomRepository roomRepository, ISeatStatusRepository seatStatusRepository,
            MyDbContext context) 
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _seatStatus = seatStatusRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSeats(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _seatRepository.GetSeats(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the seat.");
            }
        }

        //[HttpGet("{id}")]
        //public IActionResult GetSeat(int id)
        //{
        //    if (!_seatRepository.SeatExists(id))
        //        return NotFound();

        //    var seat = _seatRepository.GetSeat(id);

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(seat);
        //}


        [HttpPost]
        public IActionResult CreateSeat( [FromBody] SeatDto seatCreate)
        {
            if (seatCreate == null)
                return BadRequest();
            var seat = _seatRepository.GetSeatsToCheck()
                .Where(item => item.Name.Trim().ToUpper() == seatCreate.Name.TrimEnd().ToUpper() &&
                item.RoomId == seatCreate.RoomId && item.Column == seatCreate.Column && item.Row == seatCreate.Row).FirstOrDefault();
            if (seat != null)
            {
                ModelState.AddModelError("", "Seat already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var seatMap = _mapper.Map<Seat>(seatCreate);
            seatMap.Room = _roomRepository.GetRoomToCheck(seatCreate.RoomId);

            if (!_seatRepository.CreateSeat(seatMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSeat(int id, [FromBody] SeatDto seatUpdate)
        {
            if (seatUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var existingSeat = _context.Seats.Where(item => item.Id == id).FirstOrDefault();
            if (existingSeat == null)
                return NotFound();

            _mapper.Map(seatUpdate, existingSeat);
            existingSeat.Room = _roomRepository.GetRoomToCheck(seatUpdate.RoomId);

            if (!_seatRepository.UpdateSeat(existingSeat))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeat(int id)
        {
            if (!_seatRepository.SeatExists(id))
            {
                return NotFound();
            }

            _seatRepository.DeleteSeat(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
