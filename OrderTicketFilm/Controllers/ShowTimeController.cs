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
        public IActionResult GetShowTimes()
        {
            try
            {
                var result = _showTimeRepository.GetShowTimes();
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

        [HttpPost]
        public IActionResult CreateShowTime([FromQuery] int roomId, [FromQuery] int filmId, [FromBody] ShowTimeDto showTimeCreate)
        {
            if (showTimeCreate == null)
                return BadRequest();
            var showTime = _showTimeRepository.GetShowTimesToCheck()
                .Where(item => item.Film.Id == filmId &&
                item.Room.Id == roomId)
                .FirstOrDefault();
            if (showTime != null)
            {
                ModelState.AddModelError("", "ShowTime already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var showTimeMap = _mapper.Map<ShowTime>(showTimeCreate);
            showTimeMap.Room = _roomRepository.GetRoomToCheck(roomId);
            showTimeMap.Film = _filmRepository.GetFilmToCheck(filmId);

            if (!_showTimeRepository.CreateShowTime(showTimeMap))
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
