using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public static int PAGE_SIZE {  get; set; } = 10;

        public UserRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateUser(UserDto userCreate, User user)
        {
            if(userCreate.Roles.Any())
            {
                foreach (RoleDto itemRole in userCreate.Roles)
                {
                    var role = _context.Roles.Where(item => item.Id == itemRole.Id).FirstOrDefault();
                    var userRole = new UserRole()
                    {
                        Role = role,
                        User = user,
                    };
                    _context.Add(userRole);
                }
            }
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
            var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(u => u.Id == id);
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                UserName = user.UserName,
                Roles = user.UserRoles.Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    // Ánh xạ các thuộc tính khác của Role nếu có
                }).ToList()
            };
        }

        public UserDto GetUserByPhone(string phone)
        {
            var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(u => u.Phone == phone);
            if (user == null)
            {
                // Handle the case when the user is not found
                return null; // Or throw an exception, depending on your requirement
            }
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                UserName = user.UserName,
                Roles = user.UserRoles.Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    // Ánh xạ các thuộc tính khác của Role nếu có
                }).ToList()
            };
        }

        public ICollection<UserDto> GetUsers(int page)
        {
            var users = _context.Users.Include(item => item.UserRoles).ThenInclude(ur => ur.Role).ToList();
            var user = users.Select(item => new UserDto
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
                UserName = item.UserName,
                Roles = item.UserRoles?.Select(itemRole => new RoleDto 
                {
                    Id = itemRole.Role.Id,
                    Name = itemRole.Role.Name,
                }).ToList()
            }).ToList();
            var result = PaginatedList<UserDto>.Create(user.AsQueryable(), page, PAGE_SIZE);
            return result;
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

        public bool UpdateUser( UserDto userUpdate, User user)
        {
            if (userUpdate.Roles.Any())
            {
                foreach (RoleDto itemRole in userUpdate.Roles)
                {
                    var role = _context.Roles.Where(item => item.Id == itemRole.Id).FirstOrDefault();
                    var userRole = new UserRole()
                    {
                        Role = role,
                        User = user,
                    };
                    _context.Update(userRole);
                }
            }
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(p => p.Id == id);
        }

        public ICollection<UserDto> GetUsersByName(string name, int page)
        {
            var users = _context.Users.Where(e => e.Name.Contains(name));
            var user = users.Select(item => new UserDto
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
                UserName = item.UserName,
                Roles = item.UserRoles.Select(itemRole => new RoleDto
                {
                    Id = itemRole.Role.Id,
                    Name = itemRole.Role.Name,
                }).ToList()
            }).ToList();
            var result = PaginatedList<UserDto>.Create(user.AsQueryable(), page, PAGE_SIZE);
            return result;
        }
    }
}
