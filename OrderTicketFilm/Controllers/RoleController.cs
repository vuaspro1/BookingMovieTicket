using AutoMapper;
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
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            try
            {
                var result = _roleRepository.GetRoles();
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

        [HttpGet("getUserByRoleId")]
        public IActionResult GetUserByRoleId(int id)
        {
            if (!_roleRepository.RoleExists(id))
                return NotFound();
            var users = _roleRepository.GetUserByRole(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] RoleDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest();
            var role = _roleRepository.GetRoles()
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
            if (id != roleUpdate.Id)
                return BadRequest();
            if (!_roleRepository.RoleExists(id))
                return NotFound();
            if (!ModelState.IsValid) return BadRequest();

            var roleMap = _mapper.Map<Role>(roleUpdate);

            if (!_roleRepository.UpdateRole(roleMap))
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
