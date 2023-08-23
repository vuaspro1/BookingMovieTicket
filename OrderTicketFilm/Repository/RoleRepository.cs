using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public RoleView GetRole(int id)
        {
            var role = _context.Roles.Where(item => item.Id == id).FirstOrDefault();
            return new RoleView
            {
                Id = role.Id,
                Name = role.Name,
            };
        }

        public PaginationDTO<RoleView> GetRoles(int page, int pageSize)
        {
            PaginationDTO<RoleView> pagination = new PaginationDTO<RoleView>();
            var roles = _context.Roles.OrderBy(c => c.Id).ToList();
            var role = roles.Select(item => new RoleView
            {
                Id=item.Id,
                Name = item.Name,
            }).ToList();
            var result = PaginatedList<RoleView>.Create(role.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = role.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public ICollection<RoleDto> GetRolesByUser(int id)
        {
            var query = _context.UserRoles.Where(item => item.UserId == id).Select(c => c.Role).ToList();
            return _mapper.Map<List<RoleDto>>(query);
        }

        public Role GetRoleToCheck(int id)
        {
            var role = _context.Roles.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Role>(role);
        }

        public PaginationDTO<UserView> GetUserByRole(int id, int page, int pageSize)
        {
            PaginationDTO<UserView> pagination = new PaginationDTO<UserView>();
            var query = _context.Users.Include(item => item.UserRoles).ThenInclude(item => item.Role)
                .Where(item => item.UserRoles.Any(item => item.RoleId == id)).ToList();
            var user = query.Select(item => new UserView
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
                UserName = item.UserName,
                Roles = item.UserRoles?.Select(itemRole => new RoleView
                {
                    Id = itemRole.Role.Id,
                    Name = itemRole.Role.Name,
                }).ToList()
            }).ToList();
            var result = PaginatedList<UserView>.Create(user.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = user.Count();
            pagination.pageSize = pageSize;
            return pagination;
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
