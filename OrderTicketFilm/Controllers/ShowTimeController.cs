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
    public class ShowTimeController : Controller
    {
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public ShowTimeController(IShowTimeRepository showTimeRepository, IMapper mapper,
            IFilmRepository filmRepository, IRoomRepository roomRepository)
        {
            _showTimeRepository = showTimeRepository;
            _filmRepository = filmRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetShowTimes(int page)
        {
            try
            {
                var result = _showTimeRepository.GetShowTimes(page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the showtime.");
            }
        }

        [HttpGet("getShowTimesOfNow")]
        public IActionResult GetShowTimesOfNow(int page)
        {
            try
            {
                var result = _showTimeRepository.GetShowTimesOfNow(page);
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

        [HttpGet("getShowTimesByDay")]
        public IActionResult GetShowTimesByDay(DateTime startDate, DateTime endDate, int page)
        {
            var showTime = _showTimeRepository.GetShowTimesByDay(startDate, endDate, page);

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

            var showTimeMap = _mapper.Map<ShowTime>(showTimeCreate);
            showTimeMap.Film = _filmRepository.GetFilmToCheck(showTimeCreate.FilmId);
            showTimeMap.Room = _roomRepository.GetRoomToCheck(showTimeCreate.RoomId);

            var existingShowTime = _showTimeRepository.GetShowTimesToCheck().FirstOrDefault(item =>
            item.Film.Id == showTimeCreate.FilmId &&
            item.Room.Id == showTimeCreate.RoomId &&
            item.Time == showTimeCreate.Time);

            if (existingShowTime != null)
            {
                ModelState.AddModelError("", "ShowTime already exists");
                return BadRequest(ModelState);
            }

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

            if (id != showTimeUpdate.Id)
                return BadRequest(ModelState);

            if (!_showTimeRepository.ShowTimeExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var showTimeMap = _mapper.Map<ShowTime>(showTimeUpdate);
            showTimeMap.Film = _filmRepository.GetFilmToCheck(showTimeUpdate.FilmId);
            showTimeMap.Room = _roomRepository.GetRoomToCheck(showTimeUpdate.RoomId);

            if (!_showTimeRepository.UpdateShowTime(showTimeMap))
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
