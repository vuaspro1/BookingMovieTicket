using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using OrderTicketFilm.Repository;
using System;

namespace OrderTicketFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;

        public RoleController(IRoleRepository roleRepository, IMapper mapper,
            MyDbContext context)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRoles(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = _roleRepository.GetRoles(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("We can't get the role.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            if (!_roleRepository.RoleExists(id))
                return NotFound();

            var role = _roleRepository.GetRole(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(role);
        }

        [HttpGet("{id}/users")]
        public IActionResult GetUserByRoleId(int id, int page = 0, int pageSize = 10)
        {
            if (!_roleRepository.RoleExists(id))
                return NotFound();
            var users = _roleRepository.GetUserByRole(id, page, pageSize != 0 ? pageSize : 10);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] RoleDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest();
            var role = _context.Roles
                .Where(item => item.Name.Trim().ToUpper() == roleCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (role != null)
            {
                ModelState.AddModelError("", "This role already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = _mapper.Map<Role>(roleCreate);

            if (!_roleRepository.CreateRole(roleMap))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] RoleDto roleUpdate)
        {
            if (roleUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var role = _context.Roles
                .Where(item => item.Name.Trim().ToUpper() == roleUpdate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (role != null)
            {
                ModelState.AddModelError("", "This role already exists");
                return BadRequest(ModelState);
            }

            var existingRole = _context.Roles.FirstOrDefault(item => item.Id == id);
            if (existingRole == null) 
                return NotFound();

            _mapper.Map(roleUpdate, existingRole);

            if (!_roleRepository.UpdateRole(existingRole))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            if (!_roleRepository.RoleExists(id))
            {
                return NotFound();
            }

            _roleRepository.DeleteRole(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
