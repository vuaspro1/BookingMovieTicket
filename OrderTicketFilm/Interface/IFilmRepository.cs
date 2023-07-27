using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;
using System.Diagnostics.Metrics;

namespace OrderTicketFilm.Interface
{
    public interface IFilmRepository
    {
        ICollection<FilmDto> GetFilms();
        FilmDto GetFilm(int id);
        Film GetFilmToCheck(int id);
        List<FilmDto> GetFilmByName(string name);
        List<ShowTimeDto> GetShowTimesByAFilm(int fimlid);
        bool FilmExists(int id);
        bool CreateFilm(Film film);
        bool UpdateFilm(Film film);
        bool DeleteFilm(int id);
        bool Save();
    }
}
