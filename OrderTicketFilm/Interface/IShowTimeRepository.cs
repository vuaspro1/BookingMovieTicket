using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IShowTimeRepository
    {
        ICollection<ShowTimeDto> GetShowTimes();
        ICollection<ShowTime> GetShowTimesToCheck();
        ShowTimeDto GetShowTime(int id);
        ShowTime GetShowTimeToCheck(int id);
        bool ShowTimeExists(int id);
        bool CreateShowTime(ShowTime showTime);
        bool UpdateShowTime(ShowTime showTime);
        bool DeleteShowTime(int id);
        bool Save();
    }
}
