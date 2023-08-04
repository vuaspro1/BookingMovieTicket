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
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomController(IRoomRepository roomRepository, IMapper mapper) 
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRooms(int page)
        {
            try
            {
                var result = _roomRepository.GetRooms(page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the room.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            if (!_roomRepository.RoomExists(id))
                return NotFound();

            var room = _roomRepository.GetRoom(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(room);
        }

        [HttpGet("getSeatsByARoom")]
        public IActionResult GetSeatsByARoom(int id)
        {
            if (!_roomRepository.RoomExists(id))
                return NotFound();

            var seats = _roomRepository.GetSeatsByARoom(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(seats);
        }

        [HttpGet("getShowTimesByARoom")]
        public IActionResult GetShowTimesByARoom(int id, int page)
        {
            if (!_roomRepository.RoomExists(id))
                return NotFound();

            var showTimes = _roomRepository.GetShowTimesByARoom(id, page);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(showTimes);
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] RoomDto roomCreate)
        {
            if (roomCreate == null)
                return BadRequest();
            var room = _roomRepository.GetRoomsToCheck()
                .Where(item => item.Name.Trim().ToUpper() == roomCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (room != null)
            {
                ModelState.AddModelError("", "This room already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roomMap = _mapper.Map<Room>(roomCreate);

            if (!_roomRepository.CreateRoom(roomMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomDto roomUpdate)
        {
            if (roomUpdate == null)
                return BadRequest();
            if (id != roomUpdate.Id)
                return BadRequest();
            if (!_roomRepository.RoomExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var roomMap = _mapper.Map<Room>(roomUpdate);

            if (!_roomRepository.UpdateRoom(roomMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteType(int id)
        {
            if (!_roomRepository.RoomExists(id))
            {
                return NotFound();
            }

            _roomRepository.DeleteRoom(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
