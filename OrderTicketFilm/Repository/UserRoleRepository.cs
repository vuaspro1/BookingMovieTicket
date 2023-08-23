using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly MyDbContext _context;

        public UserRoleRepository(MyDbContext context) 
        {
            _context = context;
        }
        public bool CreateUserRole(UserRole userRole)
        {
            _context.Add(userRole);
            return Save();
        }

        public bool DeleteUserRole(int userId, int roleId)
        {
            var deleteUserRole = _context.UserRoles.SingleOrDefault(item => item.UserId == userId && item.RoleId == roleId);
            if (deleteUserRole != null)
            {
                _context.UserRoles.Remove(deleteUserRole);
            }
            return Save();
        }

        public PaginationDTO<UserRoleDto> GetAll(int page, int pageSize)
        {
            PaginationDTO<UserRoleDto> pagination = new PaginationDTO<UserRoleDto>();
            var userRoles = _context.UserRoles.ToList();
            var useRole = userRoles.Select(item => new UserRoleDto
            {
                UserId = item.UserId,
                RoleId = item.RoleId
            }).ToList();
            var result = PaginatedList<UserRoleDto>.Create(useRole.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = useRole.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UserRoleExists(int UserId, int RoleId)
        {
            return _context.UserRoles.Any(p => p.UserId == UserId && p.RoleId == RoleId);
        }
    }
}
