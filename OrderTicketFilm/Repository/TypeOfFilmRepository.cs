using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderTicketFilm.Repository
{
    public class TypeOfFilmRepository : ITypeOfFilmRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public TypeOfFilmRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateTypeOfFilm(TypeOfFilm type)
        {
            _context.Add(type);
            return Save();
        }

        public bool DeleteTypeOfFilm(int id)
        {
            var deleteType = _context.TypeOfFilms.SingleOrDefault(item => item.Id == id);
            if (deleteType != null)
            {
                _context.TypeOfFilms.Remove(deleteType);
            }
            return Save();
        }

        public PaginationDTO<FilmView> GetFilmsByATypeOfFilm(int typeId, int page, int pageSize)
        {
            PaginationDTO<FilmView> pagination = new PaginationDTO<FilmView>();
            var films = _context.Films.Include(item => item.TypeOfFilm).Where(f => f.TypeOfFilm.Id == typeId).ToList();
            var film = films.Select(item => new FilmView
            {
                Id = item.Id,
                Name = item.Name,
                OpeningDay = item.OpeningDay,
                Director = item.Director,
                Time = item.Time,
                Image = item.Image,
                TypeId = item.TypeOfFilm != null ? item.TypeOfFilm.Id : 0,
                TypeName = item.TypeOfFilm != null ? item.TypeOfFilm.Name : "Unknown",
                Description = item.Description,
            }).ToList();
            var result = PaginatedList<FilmView>.Create(film.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = film.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public TypeOfFilm GetType(int id)
        {
            var type = _context.TypeOfFilms.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<TypeOfFilm>(type);
        }

        public TypeOfFilmView GetTypeOfFilm(int id)
        {
            var type = _context.TypeOfFilms.Where(item => item.Id == id).FirstOrDefault();
            return new TypeOfFilmView
            {
                Id = type.Id,
                Name = type.Name,
            };
        }

        public PaginationDTO<TypeOfFilmView> GetTypeOfFilms(int page, int pageSize)
        {
            PaginationDTO<TypeOfFilmView> pagination = new PaginationDTO<TypeOfFilmView>();
            var types = _context.TypeOfFilms.OrderBy(c => c.Id).ToList();
            var type = types.Select(item => new TypeOfFilmView
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();
            var result = PaginatedList<TypeOfFilmView>.Create(type.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = type.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TypeOfFilmExists(int id)
        {
            return _context.TypeOfFilms.Any(c => c.Id == id);
        }

        public bool UpdateTypeOfFilm(TypeOfFilm type)
        {
            _context.Update(type);
            return Save();
        }

        public ICollection<TypeOfFilm> GetTypeOfFilmsToCheck()
        {
            var types = _context.TypeOfFilms.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<TypeOfFilm>>(types);
        }
    }
}
