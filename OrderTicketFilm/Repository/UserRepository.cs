using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System.Security.Cryptography;
using System.Text;

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
        public bool CreateUser( User user)
        {
            //if(userCreate.Roles.Any())
            //{
            //    foreach (RoleDto itemRole in userCreate.Roles)
            //    {
            //        var role = _context.Roles.Where(item => item.Id == itemRole.Id).FirstOrDefault();
            //        var userRole = new UserRole()
            //        {
            //            Role = role,
            //            User = user,
            //        };
            //        _context.Add(userRole);
            //    }
            //}
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                string passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                user.Password = passwordHash;
                _context.Add(user);
            }
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

        public UserView GetUser(int id)
        {
            var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(u => u.Id == id);
            return new UserView
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                UserName = user.UserName,
                Roles = user.UserRoles?.Select(ur => new RoleView
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    // Ánh xạ các thuộc tính khác của Role nếu có
                }).ToList()
            };
        }

        public UserView GetUserByPhone(string phone)
        {
            var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(u => u.Phone == phone);
            if (user == null)
            {
                return null; 
            }
            return new UserView
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                UserName = user.UserName,
                Roles = user.UserRoles?.Select(ur => new RoleView
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    // Ánh xạ các thuộc tính khác của Role nếu có
                }).ToList() 
            };
        }

        public PaginationDTO<UserView> GetUsers(int page, int pageSize)
        {
            PaginationDTO<UserView> pagination = new PaginationDTO<UserView>();
            var users = _context.Users.Include(item => item.UserRoles).ThenInclude(item => item.Role)
                .OrderBy(c => c.Id).ToList();
            var user = users.Select(item => new UserView
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

        public bool UpdateUser(User user)
        {
            //if (userUpdate.Roles.Any())
            //{
            //    foreach (RoleDto itemRole in userUpdate.Roles)
            //    {
            //        var role = _context.Roles.Where(item => item.Id == itemRole.Id).FirstOrDefault();
            //        var userRole = new UserRole()
            //        {
            //            Role = role,
            //            User = user,
            //        };
            //        _context.Update(userRole);
            //    }
            //}
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                string passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                _context.Update(user);
            }
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(p => p.Id == id);
        }

        public PaginationDTO<UserView> GetUsersBySearch(string search, int page, int pageSize)
        {
            PaginationDTO<UserView> pagination = new PaginationDTO<UserView>();
            var users = _context.Users.Where(item => item.UserName.Trim().ToUpper().Contains(search.TrimEnd().ToUpper()) ||
            item.Name.Trim().ToUpper().Contains(search.TrimEnd().ToUpper()) || item.Address.Trim().ToUpper().Contains(search.TrimEnd().ToUpper()));
            var user = users.Select(item => new UserView
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
                UserName = item.UserName,
                Roles = item.UserRoles.Select(itemRole => new RoleView
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

        public ICollection<User> GetUsersToCheck()
        {
            var user = _context.Users.ToList();
            return _mapper.Map<List<User>>(user);
        }

        public User Authenticate(string username, string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                string passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();

                return _context.Users.FirstOrDefault(user => user.UserName == username && user.Password == passwordHash);
            }
        }
    }
}
