using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoleRepository
    {
        PaginationDTO<RoleView> GetRoles(int page, int pageSize);
        RoleView GetRole(int id);
        Role GetRoleToCheck(int id);
        PaginationDTO<UserView> GetUserByRole(int id, int page, int pageSize);
        ICollection<RoleDto> GetRolesByUser(int id);
        bool CreateRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(int id);
        bool Save();
        bool RoleExists(int id);
    }
}
