using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateUser(int roleId, User user)
        {
            var role = _context.Roles.Where(item => item.Id == roleId).FirstOrDefault();
            var userRole = new UserRole()
            {
                Role = role,
                User = user,
            };
            _context.Add(userRole);
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(int id)
        {
            var deleteUser = _context.Users.SingleOrDefault(item => item.Id == id);
            if (deleteUser != null)
            {
                _context.Users.Remove(deleteUser);
            }
            return Save();
        }

        public UserDto GetUser(int id)
        {
            var user = _context.Users.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }

        public UserDto GetUserByPhone(string phone)
        {
            var query = _context.Users.AsQueryable().Where(e => e.Phone.Contains(phone));
            return _mapper.Map<UserDto>(query);
        }

        public ICollection<UserDto> GetUsers()
        {
            var users = _context.Users.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<UserDto>>(users);
        }

        public User GetUserToCheck(int id)
        {
            var user = _context.Users.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<User>(user);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(int roleId, User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(p => p.Id == id);
        }
    }
}
