using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IUserRoleRepository
    {
        PaginationDTO<UserRoleDto> GetAll(int page, int pageSize);
        bool CreateUserRole (UserRole userRole);
        bool DeleteUserRole (int userId, int roleId);
        bool Save();
        bool UserRoleExists(int UserId, int RoleId);
    }
}
