using AutoMapper;
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

        public ICollection<FilmDto> GetFilmsByATypeOfFilm(int typeId)
        {
            var query = _context.Films.Where(f => f.TypeOfFilm.Id == typeId).ToList();
            return _mapper.Map<List<FilmDto>>(query);
        }

        public TypeOfFilm GetType(int id)
        {
            var type = _context.TypeOfFilms.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<TypeOfFilm>(type);
        }

        public TypeOfFilmDto GetTypeOfFilm(int id)
        {
            var type = _context.TypeOfFilms.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<TypeOfFilmDto>(type);
        }

        public TypeOfFilmDto GetTypeOfFilmByFilm(int filmId)
        {
            var query = _context.Films.Where(o => o.Id == filmId).Select(c => c.TypeOfFilm).FirstOrDefault();
            return _mapper.Map<TypeOfFilmDto>(query);
        }

        public ICollection<TypeOfFilmDto> GetTypeOfFilms()
        {
            var type = _context.TypeOfFilms.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<TypeOfFilmDto>>(type);
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
    }
}
