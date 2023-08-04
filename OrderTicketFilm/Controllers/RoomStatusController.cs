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
    public class RoomStatusController : Controller
    {
        private readonly IRoomStatusRepository _roomStatus;
        private readonly IMapper _mapper;

        public RoomStatusController(IRoomStatusRepository roomStatusRepository, IMapper mapper) 
        {
            _roomStatus = roomStatusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoomStatuses()
        {
            try
            {
                var result = _roomStatus.GetRoomStatuses();
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the status.");
            }
        }

        [HttpPost]
        public IActionResult CreateStatus([FromBody] RoomStatusDto roomStatus)
        {
            if (roomStatus == null)
                return BadRequest();
            var status = _roomStatus.GetRoomStatuses()
                .Where(item => item.Status.Trim().ToUpper() == roomStatus.Status.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (status != null)
            {
                ModelState.AddModelError("", "This status already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var statusMap = _mapper.Map<RoomStatus>(roomStatus);

            if (!_roomStatus.CreateRoomStatus(statusMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] RoomStatusDto roomStatus)
        {
            if (roomStatus == null)
                return BadRequest();
            if (!_roomStatus.RoomStatusExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var statusMap = _roomStatus.GetStatus(id);
            if (statusMap == null)
                return NotFound();

            statusMap.Status = roomStatus.Status;

            if (!_roomStatus.UpdateRoomStatus(statusMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStatus(int id)
        {
            if (!_roomStatus.RoomStatusExists(id))
            {
                return NotFound();
            }

            _roomStatus.DeleteRoomStatus(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
