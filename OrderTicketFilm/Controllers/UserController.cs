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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public UserController(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetUsers(int page)
        {
            try
            {
                var result = _userRepository.GetUsers(page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the user.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTypeOfFilm(int id)
        {
            if (!_userRepository.UserExists(id))
                return NotFound();

            var user = _userRepository.GetUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("getUserByPhone")]
        public IActionResult GetUserByPhone(string phone)
        {
            var user = _userRepository.GetUserByPhone(phone);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("getUsersByName")]
        public IActionResult GetUsersByName(string name, int page)
        {
            var user = _userRepository.GetUsersByName(name, page);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest();
            var user = _userRepository.GetUserByPhone(userCreate.Phone);

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userCreate, userMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            if (id != userUpdate.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(userUpdate);

            if (!_userRepository.UpdateUser(userUpdate, userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
