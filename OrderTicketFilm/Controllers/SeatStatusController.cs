using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SeatStatusController : Controller
    {
        private readonly ISeatStatusRepository _seatStatus;
        private readonly IMapper _mapper;

        public SeatStatusController(ISeatStatusRepository seatStatusRepository,IMapper mapper) 
        {
            _seatStatus = seatStatusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSeatStatuses(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _seatStatus.GetSeatStatuses(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the status.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSeatStatus(int id)
        {
            if (!_seatStatus.SeatStatusExists(id))
                return NotFound();

            var seatStatus = _seatStatus.GetStatus(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(seatStatus);
        }

        [HttpPost]
        public IActionResult CreateStatus([FromBody] SeatStatusDto seatStatusDto)
        {
            if (seatStatusDto == null)
                return BadRequest();
            var status = _seatStatus.GetSeatStatusesToCheck()
                .Where(item => item.Status.Trim().ToUpper() == seatStatusDto.Status.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (status != null)
            {
                ModelState.AddModelError("", "This status already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var statusMap = _mapper.Map<SeatStatus>(seatStatusDto);

            if (!_seatStatus.CreateSeatStatus(statusMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] SeatStatusUpdate seatStatusUpdate)
        {
            if (seatStatusUpdate == null)
                return BadRequest();
            if (!_seatStatus.SeatStatusExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var statusMap = _seatStatus.GetStatusToCheck(id);
            if (statusMap == null)
                return NotFound();

            statusMap.Status = seatStatusUpdate.Status;

            if (!_seatStatus.UpdateSeatStatus(statusMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStatus(int id)
        {
            if (!_seatStatus.SeatStatusExists(id))
            {
                return NotFound();
            }

            _seatStatus.DeleteSeatStatus(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
