using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IUserRepository
    {
        PaginationDTO<UserView> GetUsers(int page, int pageSize);
        ICollection<User> GetUsersToCheck();
        PaginationDTO<UserView> GetUsersBySearch(string search, int page, int pageSize);
        UserView GetUser(int id);
        User GetUserToCheck(int id);
        UserView GetUserByPhone(string phone);
        bool UserExists(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int id);
        bool Save();
        User Authenticate(string username, string password);
    }
}
