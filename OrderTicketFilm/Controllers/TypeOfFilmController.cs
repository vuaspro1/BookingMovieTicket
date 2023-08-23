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
    public class TypeOfFilmController : Controller
    {
        private readonly ITypeOfFilmRepository _typeOfFilmRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;

        public TypeOfFilmController(ITypeOfFilmRepository typeOfFilmRepository, IMapper mapper,
            MyDbContext context)
        {
            _typeOfFilmRepository = typeOfFilmRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTypeOfFilms(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _typeOfFilmRepository.GetTypeOfFilms(page, pageSize != 0 ? pageSize : 10);
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

        [HttpGet("{typeId}/films")]
        public IActionResult GetFilmsByATypeOfFilm(int typeId, int page = 0, int pageSize = 10)
        {
            var type = _typeOfFilmRepository.GetFilmsByATypeOfFilm(typeId, page, pageSize != 0 ? pageSize : 10);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(type);
        }

        [HttpPost]
        public IActionResult CreateType([FromBody] TypeOfFilmDto typeOfFilmCreate)
        {
            if (typeOfFilmCreate == null)
                return BadRequest();
            var type = _typeOfFilmRepository.GetTypeOfFilmsToCheck()
                .Where(item => item.Name.Trim().ToUpper() == typeOfFilmCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (type != null)
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
        public IActionResult UpdateType (int id, [FromBody] TypeOfFilmDto typeOfFilmUpdate)
        {
            if (typeOfFilmUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var type = _typeOfFilmRepository.GetTypeOfFilmsToCheck()
                .Where(item => item.Name.Trim().ToUpper() == typeOfFilmUpdate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (type != null)
            {
                ModelState.AddModelError("", "This type already exists");
                return BadRequest(ModelState);
            }

            var existingType = _context.TypeOfFilms.FirstOrDefault(item => item.Id == id);
            if (existingType == null) 
                return NotFound();

            _mapper.Map(typeOfFilmUpdate, existingType);

            if (!_typeOfFilmRepository.UpdateTypeOfFilm(existingType))
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

            _typeOfFilmRepository.DeleteTypeOfFilm(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
