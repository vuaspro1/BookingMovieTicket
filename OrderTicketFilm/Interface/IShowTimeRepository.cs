using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IShowTimeRepository
    {
        ICollection<ShowTimeView> GetShowTimes(int page);
        ICollection<ShowTimeView> GetShowTimesOfNow(int page);
        ICollection<ShowTimeView> GetShowTimesByDay(DateTime startDate, DateTime endDate, int page);
        ICollection<ShowTime> GetShowTimesToCheck();
        ShowTimeView GetShowTime(int id);
        ShowTime GetShowTimeToCheck(int id);
        bool ShowTimeExists(int id);
        bool CreateShowTime( ShowTime showTime);
        bool UpdateShowTime( ShowTime showTime);
        bool DeleteShowTime(int id);
        bool Save();
    }
}
