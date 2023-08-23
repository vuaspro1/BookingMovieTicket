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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;

        public UserController(IUserRepository userRepository, IMapper mapper,
            MyDbContext context) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _userRepository.GetUsers(page, pageSize != 0 ? pageSize : 10);
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

        [HttpGet("{search}/users")]
        public IActionResult GetUsersBySearch(string search, int page = 0, int pageSize = 10)
        {
            var user = _userRepository.GetUsersBySearch(search, page, pageSize != 0 ? pageSize : 10);
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
            var userByPhone = _userRepository.GetUserByPhone(userCreate.Phone);
            var userByUserName = _userRepository.GetUsersToCheck().FirstOrDefault(item => item.UserName == userCreate.UserName);

            if (userByPhone != null)
            {
                ModelState.AddModelError("", "Phone already exists");
                return BadRequest(ModelState);
            }

            if (userByUserName != null)
            {
                ModelState.AddModelError("", "UserName already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdate userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            var user = _userRepository.GetUserByPhone(userUpdate.Phone);

            if (user != null )
            { 
                ModelState.AddModelError("", "Phone already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var existingUser = _context.Users.FirstOrDefault(item => item.Id == id);
            if (existingUser == null)
                return NotFound();

            _mapper.Map(userUpdate, existingUser);
            existingUser.UserName = _userRepository.GetUser(id).UserName;

            if (!_userRepository.UpdateUser( existingUser))
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
