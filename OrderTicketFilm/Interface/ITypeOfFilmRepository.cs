using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ITypeOfFilmRepository
    {
        ICollection<TypeOfFilmDto> GetTypeOfFilms();
        TypeOfFilmDto GetTypeOfFilm(int id);
        TypeOfFilm GetType(int id);
        TypeOfFilmDto GetTypeOfFilmByFilm(int filmId);
        ICollection<FilmDto> GetFilmsByATypeOfFilm(int typeId);
        bool TypeOfFilmExists(int id);
        bool CreateTypeOfFilm(TypeOfFilm type);
        bool UpdateTypeOfFilm(TypeOfFilm type);
        bool DeleteTypeOfFilm(int id);
        bool Save();
    }
}
