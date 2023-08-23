using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;
using System.Diagnostics.Metrics;

namespace OrderTicketFilm.Interface
{
    public interface IFilmRepository
    {
        PaginationDTO<FilmView> GetFilms(int page, int pageSize);
        FilmView GetFilm(int id);
        Film GetFilmToCheck(int id);
        PaginationDTO<FilmView> GetFilmByName(string name, int page, int pageSize);
        PaginationDTO<ShowTimeView> GetShowTimesByAFilm(int fimlid, int page, int pageSize);
        bool FilmExists(int id);
        bool CreateFilm( Film film);
        bool UpdateFilm(Film film);
        bool DeleteFilm(int id);
        bool Save();
    }
}
