using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;
using System.Diagnostics.Metrics;

namespace OrderTicketFilm.Interface
{
    public interface IFilmRepository
    {
        ICollection<FilmView> GetFilms(int page);
        FilmView GetFilm(int id);
        Film GetFilmToCheck(int id);
        ICollection<FilmView> GetFilmByName(string name, int page);
        ICollection<ShowTimeView> GetShowTimesByAFilm(int fimlid, int page);
        bool FilmExists(int id);
        bool CreateFilm( Film film);
        bool UpdateFilm(Film film);
        bool DeleteFilm(int id);
        bool Save();
    }
}
