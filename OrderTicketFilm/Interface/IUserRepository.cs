using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IUserRepository
    {
        ICollection<UserDto> GetUsers(int page);
        ICollection<UserDto> GetUsersByName(string name, int page);
        UserDto GetUser(int id);
        User GetUserToCheck(int id);
        UserDto GetUserByPhone(string phone);
        bool UserExists(int id);
        bool CreateUser(UserDto userCreate, User user);
        bool UpdateUser(UserDto userUpdate, User user);
        bool DeleteUser(int id);
        bool Save();
    }
}
