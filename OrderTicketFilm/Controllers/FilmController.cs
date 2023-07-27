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
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        private readonly ITypeOfFilmRepository _type;

        public FilmController(IFilmRepository filmRepository, ITypeOfFilmRepository typeOfFilmRepository, IMapper mapper)
        {
            _type = typeOfFilmRepository;
            _filmRepository = filmRepository;
            _mapper = mapper; 
        }

        [HttpGet]
        public IActionResult GetFilms()
        {
            try
            {
                var result = _filmRepository.GetFilms();
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

        [HttpGet("getFilmByName")]
        public IActionResult GetFilmByName(string name)
        {
            var film = _filmRepository.GetFilmByName(name);
            if (!film.Any())
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(film);
        }

        [HttpGet("getFilmsByATypeOfFilm")]
        public IActionResult GetFilmsByATypeOfFilm(int typeId)
        {
            var type = _type.GetFilmsByATypeOfFilm(typeId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(type);
        }

        [HttpGet("getShowTimesByAFilm")]
        public IActionResult GetShowTimesByAFilm(int id)
        {
            if (!_filmRepository.FilmExists(id))
                return NotFound();

            var showTimes = _filmRepository.GetShowTimesByAFilm(id);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(showTimes);
        }

        [HttpPost]
        public IActionResult CreateFilm([FromQuery] int typeId, [FromBody] FilmDto filmCreate)
        {
            if (filmCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filmMap = _mapper.Map<Film>(filmCreate);

            filmMap.TypeOfFilm = _type.GetType(typeId);

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

            if (id != filmUpdate.Id)
                return BadRequest(ModelState);

            if (!_filmRepository.FilmExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var filmMap = _mapper.Map<Film>(filmUpdate);

            if (!_filmRepository.UpdateFilm(filmMap))
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
