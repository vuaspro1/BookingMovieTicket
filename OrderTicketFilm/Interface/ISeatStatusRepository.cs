using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ISeatStatusRepository
    {
        ICollection<SeatStatusDto> GetSeatStatuses();
        SeatStatus GetStatus(int id);
        bool SeatStatusExists(int id);
        bool CreateSeatStatus(SeatStatus seatStatus);
        bool DeleteSeatStatus(int id);
        bool UpdateSeatStatus(SeatStatus seatStatus);
        bool Save();
    }
}
