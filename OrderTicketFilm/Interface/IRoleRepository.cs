using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoleRepository
    {
        ICollection<RoleDto> GetRoles();
        RoleDto GetRole(int id);
        List<UserDto> GetUserByRole(int id);
        ICollection<Role> GetRolesByUser(int id);
        bool CreateRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(int id);
        bool Save();
        bool RoleExists(int id);
    }
}
