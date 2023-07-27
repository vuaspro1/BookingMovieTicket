using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateRole(Role role)
        {
            _context.Add(role);
            return Save();
        }

        public bool DeleteRole(int id)
        {
            var deleteRole = _context.Roles.SingleOrDefault(item => item.Id == id);
            if (deleteRole != null)
            {
                _context.Roles.Remove(deleteRole);
            }
            return Save();
        }

        public RoleDto GetRole(int id)
        {
            var role = _context.Roles.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<RoleDto>(role);
        }

        public ICollection<RoleDto> GetRoles()
        {
            var roles = _context.Roles.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public ICollection<Role> GetRolesByUser(int id)
        {
            var query = _context.UserRoles.Where(item => item.UserId == id).Select(c => c.Role).ToList();
            return _mapper.Map<List<Role>>(query);
        }

        public List<UserDto> GetUserByRole(int id)
        {
            var query = _context.UserRoles.Where(item => item.RoleId == id).Select(c => c.User).ToList();
            return _mapper.Map<List<UserDto>>(query);
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRole(Role role)
        {
            _context.Update(role);
            return Save();
        }
    }
}
