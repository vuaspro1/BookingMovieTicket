using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderTicketFilm.Repository
{
    public class FilmRepository : IFilmRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public static int PAGE_SIZE { get; set; } = 10;

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

        public FilmView GetFilm(int id)
        {
            var query = _context.Films.Include(item => item.TypeOfFilm).FirstOrDefault(f => f.Id == id);
            return new FilmView
            {
                Id = query.Id,
                Name = query.Name,
                OpeningDay = query.OpeningDay,
                Director = query.Director,
                Time = query.Time,
                Image = query.Image,
                TypeId = query.TypeOfFilm != null ? query.TypeOfFilm.Id : 0,
                TypeName = query.TypeOfFilm != null ? query.TypeOfFilm.Name : "Unknown",
            };
        }

        public ICollection<FilmView> GetFilmByName(string name, int page)
        {
            var films = _context.Films.Include(item => item.TypeOfFilm).AsQueryable().Where(e => e.Name.Contains(name));
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
            }).ToList();
            var result = PaginatedList<FilmView>.Create(film.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public ICollection<FilmView> GetFilms(int page)
        {
            var films = _context.Films.Include(item => item.TypeOfFilm).ToList();
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
            }).ToList();
            var result = PaginatedList<FilmView>.Create(film.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public Film GetFilmToCheck(int id)
        {
            var film = _context.Films.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Film>(film);
        }

        public ICollection<ShowTimeView> GetShowTimesByAFilm(int fimlid, int page)
        {
            var showTimes = _context.ShowTimes.Where(e => e.Film.Id == fimlid).OrderBy(item => item.Time.Day).ToList();
            var showTime = showTimes.Select(item => new ShowTimeView
            {
                Id = item.Id,
                RoomId = item.Room.Id,
                FilmId = item.Film.Id,
                Time = item.Time,
                FilmName = item.Film.Name,
                RoomName = item.Room.Name,
                Duration = item.Film.Time,
                Image = item.Film.Image,
            }).ToList();
            var paginatedList = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, PAGE_SIZE);
            return paginatedList;
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
