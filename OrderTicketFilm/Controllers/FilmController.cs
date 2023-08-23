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
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;
        private readonly ITypeOfFilmRepository _type;

        public FilmController(IFilmRepository filmRepository, ITypeOfFilmRepository typeOfFilmRepository, 
            IMapper mapper, MyDbContext context)
        {
            _type = typeOfFilmRepository;
            _filmRepository = filmRepository;
            _mapper = mapper; 
            _context = context;
        }

        [HttpGet]
        public IActionResult GetFilms(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _filmRepository.GetFilms(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the film.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFilm(int id)
        {
            if (!_filmRepository.FilmExists(id))
                return NotFound();

            var film = _filmRepository.GetFilm(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(film);
        }

        [HttpGet("{name}/films")]
        public IActionResult GetFilmByName(string name, int page = 0, int pageSize = 10)
        {
            var film = _filmRepository.GetFilmByName(name, page, pageSize != 0 ? pageSize : 10);
            if (film == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(film);
        }

        [HttpGet("{id}/showTimes")]
        public IActionResult GetShowTimesByAFilm(int id, int page = 0, int pageSize = 10)
        {
            if (!_filmRepository.FilmExists(id))
                return NotFound();

            var showTimes = _filmRepository.GetShowTimesByAFilm(id, page, pageSize != 0 ? pageSize : 10);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(showTimes);
        }

        [HttpPost]
        public IActionResult CreateFilm([FromBody] FilmDto filmCreate)
        {
            if (filmCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filmMap = _mapper.Map<Film>(filmCreate);
            filmMap.TypeOfFilm = _type.GetType(filmCreate.TypeId);

            if (!_filmRepository.CreateFilm(filmMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFilm (int id, [FromBody] FilmDto filmUpdate)
        {
            if (filmUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var existingFilm = _context.Films.FirstOrDefault(item => item.Id == id);
            if (existingFilm == null)
                return NotFound();

            _mapper.Map(filmUpdate, existingFilm);
            existingFilm.TypeOfFilm = _type.GetType(filmUpdate.TypeId);

            if (!_filmRepository.UpdateFilm(existingFilm))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFilm(int id)
        {
            if (!_filmRepository.FilmExists(id))
            {
                return NotFound();
            }

            _filmRepository.DeleteFilm(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }

        //[HttpDelete("/DeleteFilmsByTypeOfFilm/{typeId}")]
        //public IActionResult DeleteFilmsByTypeOfFilm (int typeId)
        //{
        //    if (!_filmRepository.FilmExists(typeId))
        //        return NotFound();

        //    var typeToDelete = _type.GetFilms(typeId).ToList();
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    if (!_filmRepository.DeleteFilms(typeToDelete))
        //    {
        //        ModelState.AddModelError("", "error deleting reviews");
        //        return StatusCode(500, ModelState);
        //    }
        //    return Ok("Successfully");
        //}
    }
}
