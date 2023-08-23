using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ITypeOfFilmRepository
    {
        PaginationDTO<TypeOfFilmView> GetTypeOfFilms(int page, int pageSize);
        ICollection<TypeOfFilm> GetTypeOfFilmsToCheck();
        TypeOfFilmView GetTypeOfFilm(int id);
        TypeOfFilm GetType(int id);
        PaginationDTO<FilmView> GetFilmsByATypeOfFilm(int typeId, int page, int pageSize);
        bool TypeOfFilmExists(int id);
        bool CreateTypeOfFilm(TypeOfFilm type);
        bool UpdateTypeOfFilm(TypeOfFilm type);
        bool DeleteTypeOfFilm(int id);
        bool Save();
    }
}
