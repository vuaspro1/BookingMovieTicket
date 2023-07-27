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
    public class TypeOfFilmController : Controller
    {
        private readonly ITypeOfFilmRepository _typeOfFilmRepository;
        private readonly IMapper _mapper;

        public TypeOfFilmController(ITypeOfFilmRepository typeOfFilmRepository, IMapper mapper)
        {
            _typeOfFilmRepository = typeOfFilmRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTypeOfFilms()
        {
            try
            {
                var result = _typeOfFilmRepository.GetTypeOfFilms();
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the type.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTypeOfFilm(int id)
        {
            if (!_typeOfFilmRepository.TypeOfFilmExists(id))
                return NotFound();

            var type = _typeOfFilmRepository.GetTypeOfFilm(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(type);
        }

        [HttpGet("/films/{filmId}")]
        public IActionResult GetTypeOfFilmByFilm(int filmId)
        {
            var type = _typeOfFilmRepository.GetTypeOfFilmByFilm(filmId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(type);
        }

        [HttpPost]
        public IActionResult CreateType([FromBody] TypeOfFilm typeOfFilmCreate)
        {
            if (typeOfFilmCreate == null)
                return BadRequest();
            var customer = _typeOfFilmRepository.GetTypeOfFilms()
                .Where(item => item.Name.Trim().ToUpper() == typeOfFilmCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (customer != null)
            {
                ModelState.AddModelError("", "This type already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var typeMap = _mapper.Map<TypeOfFilm>(typeOfFilmCreate);

            if (!_typeOfFilmRepository.CreateTypeOfFilm(typeMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateType (int id, [FromBody] TypeOfFilm typeOfFilmUpdate)
        {
            if (typeOfFilmUpdate == null)
                return BadRequest();
            if (id != typeOfFilmUpdate.Id)
                return BadRequest();
            if (!_typeOfFilmRepository.TypeOfFilmExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var typeMap = _mapper.Map<TypeOfFilm>(typeOfFilmUpdate);

            if (!_typeOfFilmRepository.UpdateTypeOfFilm(typeMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteType (int id)
        {
            if (!_typeOfFilmRepository.TypeOfFilmExists(id))
            {
                return NotFound();
            }

            //var filmsToDelete = _typeOfFilmRepository.GetFilmsByAType(id);
            //_typeOfFilmRepository.DeleteTypeOfFilm(filmsToDelete.ToList());
            _typeOfFilmRepository.DeleteTypeOfFilm(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
