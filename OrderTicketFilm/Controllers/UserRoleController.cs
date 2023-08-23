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
    public class UserRoleController : Controller
    {
        private readonly IUserRoleRepository _userRole;
        private readonly IMapper _mapper;
        private readonly IUserRepository _user;
        private readonly IRoleRepository _role;

        public UserRoleController(IUserRoleRepository userRoleRepository, IMapper mapper,
            IUserRepository userRepository, IRoleRepository roleRepository) 
        {
            _userRole = userRoleRepository;
            _mapper = mapper;
            _user = userRepository;
            _role = roleRepository;
        }

        [HttpGet]
        public IActionResult GetUserRoles(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _userRole.GetAll(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the type.");
            }
        }

        [HttpPost]
        public IActionResult CreateUserRole([FromBody]UserRoleDto userRoleDto)
        {
            if (userRoleDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRoleMap = _mapper.Map<UserRole>(userRoleDto);
            userRoleMap.User = _user.GetUserToCheck(userRoleDto.UserId);
            userRoleMap.Role = _role.GetRoleToCheck(userRoleDto.RoleId);

            if (!_userRole.CreateUserRole(userRoleMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("DeleteUserRole")]
        public IActionResult DeleteUserRole(int userId, int roleId)
        {
            if (!_userRole.UserRoleExists(userId,roleId))
            {
                return NotFound();
            }

            _userRole.DeleteUserRole(userId,roleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
