using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IShowTimeRepository
    {
        PaginationDTO<ShowTimeView> GetShowTimes(int page, int pageSize);
        PaginationDTO<ShowTimeView> GetShowTimesByDay(DateTime startDate, DateTime endDate, int page, int pageSize);
        ICollection<ShowTime> GetShowTimesToCheck();
        ShowTimeView GetShowTime(int id);
        ShowTime GetShowTimeToCheck(int? id);
        bool ShowTimeExists(int id);
        bool CreateShowTime( ShowTime showTime);
        bool UpdateShowTime( ShowTime showTime);
        bool DeleteShowTime(int id);
        bool Save();
    }
}
