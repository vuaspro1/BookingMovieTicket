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
    public class ShowTimeController : Controller
    {
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;
        private readonly ITicketRepository _ticketRepository;

        public ShowTimeController(IShowTimeRepository showTimeRepository, IMapper mapper,
            IFilmRepository filmRepository, IRoomRepository roomRepository,
            MyDbContext context, ITicketRepository ticketRepository)
        {
            _showTimeRepository = showTimeRepository;
            _filmRepository = filmRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _context = context;
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public IActionResult GetShowTimes(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _showTimeRepository.GetShowTimes(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the showtime.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetShowTime(int id)
        {
            if (!_showTimeRepository.ShowTimeExists(id))
                return NotFound();

            var showTime = _showTimeRepository.GetShowTime(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(showTime);
        }

        [HttpGet("{showTimeId}/seats")]
        public IActionResult GetSeatsByShowTime(int showTimeId)
        {
            LayoutSeat result = new LayoutSeat();
            if (!_showTimeRepository.ShowTimeExists(showTimeId))
                return NotFound();

            //da dat // trong 
            Enums status = new Enums();

            SeatStatus seatStatusOrder = _context.SeatStatuses.FirstOrDefault(item => item.Code == status.seatOrderInfo.Code);
            SeatStatus seatStatusEmpty = _context.SeatStatuses.FirstOrDefault(item => item.Code == status.seatEmptyInfo.Code);

            var ticketsByShowTime = _ticketRepository.GetTicketsByShowTime(showTimeId);
            var roomByShowTime = _roomRepository.GetRoomByShowTimeId(showTimeId);
            var seatsByRoom = _roomRepository.GetSeatsByARoom(roomByShowTime.Id);

            int maxColumn = seatsByRoom.Max(seat => seat.Column);
            result.columnSeats = new List<ColumnSeat>();

            for (int col = 1; col <= maxColumn; col++)
            {
                result.columnSeats.Add(new ColumnSeat { Column = col, seatOfShowTimes = new List<SeatShow>() });
            }

            foreach ( var seat in seatsByRoom.OrderBy(item => item.Row))
            {
                var seatShow = new SeatShow
                {
                    Id = seat.Id,
                    Name = seat.Name,
                    Price = seat.Price,
                    RoomId = seat.RoomId,
                    Row = seat.Row,
                    Column = seat.Column
                };

                var ticketForSeat = ticketsByShowTime.FirstOrDefault(item => item.SeatId == seat.Id);
                if (ticketForSeat != null)
                {
                    seatShow.Code = seatStatusOrder.Code;
                    seatShow.Status = seatStatusOrder.Status;
                }
                else
                {
                    seatShow.Code = seatStatusEmpty.Code;
                    seatShow.Status = seatStatusEmpty.Status;
                }
                var columnSeat = result.columnSeats.FirstOrDefault(colSeat => colSeat.Column == seat.Column);
                if (columnSeat != null)
                {
                    columnSeat.seatOfShowTimes.Add(seatShow);
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpGet("ByDay")]
        public IActionResult GetShowTimesByDay(DateTime startDate, DateTime endDate, int page = 0, int pageSize = 10)
        {
            var showTime = _showTimeRepository.GetShowTimesByDay(startDate, endDate, page, pageSize != 0 ? pageSize : 10);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(showTime);
        }

        [HttpPost]
        public IActionResult CreateShowTime( [FromBody] ShowTimeDto showTimeCreate)
        {
            if (showTimeCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingShowTime = _showTimeRepository.GetShowTimesToCheck().FirstOrDefault(item =>
            item.Film.Id == showTimeCreate.FilmId &&
            item.Room.Id == showTimeCreate.RoomId &&
            item.Time == showTimeCreate.Time);

            if (existingShowTime != null)
            {
                ModelState.AddModelError("", "ShowTime already exists");
                return BadRequest(ModelState);
            }

            var showTimeMap = _mapper.Map<ShowTime>(showTimeCreate);
            showTimeMap.Film = _filmRepository.GetFilmToCheck(showTimeCreate.FilmId);
            showTimeMap.Room = _roomRepository.GetRoomToCheck(showTimeCreate.RoomId);

            if (!_showTimeRepository.CreateShowTime( showTimeMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShowTime(int id, [FromBody] ShowTimeDto showTimeUpdate)
        {
            if (showTimeUpdate== null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var showTime = _showTimeRepository.GetShowTimesToCheck().FirstOrDefault(item =>
            item.Film.Id == showTimeUpdate.FilmId &&
            item.Room.Id == showTimeUpdate.RoomId &&
            item.Time == showTimeUpdate.Time);

            if (showTime != null)
            {
                ModelState.AddModelError("", "ShowTime already exists");
                return BadRequest(ModelState);
            }

            var existingShowTime = _context.ShowTimes.FirstOrDefault(item => item.Id == id);
            if (existingShowTime == null)
                return NotFound();

            _mapper.Map(showTimeUpdate, existingShowTime);
            existingShowTime.Film = _filmRepository.GetFilmToCheck(showTimeUpdate.FilmId);
            existingShowTime.Room = _roomRepository.GetRoomToCheck(showTimeUpdate.RoomId);

            if (!_showTimeRepository.UpdateShowTime(existingShowTime))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShowTime(int id)
        {
            if (!_showTimeRepository.ShowTimeExists(id))
            {
                return NotFound();
            }

            _showTimeRepository.DeleteShowTime(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
