using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IUserRepository
    {
        ICollection<UserDto> GetUsers();
        UserDto GetUser(int id);
        User GetUserToCheck(int id);
        UserDto GetUserByPhone(string phone);
        bool UserExists(int id);
        bool CreateUser(int roleId, User user);
        bool UpdateUser(int roleId, User user);
        bool DeleteUser(int id);
        bool Save();
    }
}
