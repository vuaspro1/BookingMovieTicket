using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ITypeOfFilmRepository
    {
        ICollection<TypeOfFilmDto> GetTypeOfFilms();
        ICollection<TypeOfFilmDto> GetTypeOfFilmsToCheck();
        TypeOfFilmDto GetTypeOfFilm(int id);
        TypeOfFilm GetType(int id);
        ICollection<FilmView> GetFilmsByATypeOfFilm(int typeId, int page);
        bool TypeOfFilmExists(int id);
        bool CreateTypeOfFilm(TypeOfFilm type);
        bool UpdateTypeOfFilm(TypeOfFilm type);
        bool DeleteTypeOfFilm(int id);
        bool Save();
    }
}
