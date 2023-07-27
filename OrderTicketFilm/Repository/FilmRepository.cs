using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class FilmRepository : IFilmRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public FilmRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateFilm(Film film)
        {
            _context.Add(film);
            return Save();
        }

        public bool DeleteFilm(int id)
        {
            var deleteFilm = _context.Films.SingleOrDefault(item => item.Id == id);
            if (deleteFilm != null)
            {
                _context.Films.Remove(deleteFilm);
            }
            return Save();
        }

        public bool FilmExists(int id)
        {
            return _context.Films.Any(f => f.Id == id);
        }

        public FilmDto GetFilm(int id)
        {
            var film = _context.Films.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<FilmDto>(film);
        }

        public List<FilmDto> GetFilmByName(string name)
        {
            var query = _context.Films.AsQueryable().Where(e => e.Name.Contains(name));
            return _mapper.Map<List<FilmDto>>(query);
        }

        public ICollection<FilmDto> GetFilms()
        {
            var films = _context.Films.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<FilmDto>>(films);
        }

        public Film GetFilmToCheck(int id)
        {
            var film = _context.Films.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Film>(film);
        }

        public List<ShowTimeDto> GetShowTimesByAFilm(int fimlid)
        {
            var query = _context.ShowTimes.Where(e => e.Film.Id == fimlid).ToList();
            return _mapper.Map<List<ShowTimeDto>>(query);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateFilm(Film film)
        {
            _context.Update(film);
            return Save();
        }
    }
}
