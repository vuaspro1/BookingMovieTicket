using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ISeatRepository
    {
        PaginationDTO<SeatView> GetSeats(int page, int pageSize);
        ICollection<SeatDto> GetSeatsToCheck();
        SeatView GetSeat(int id);
        Seat GetSeatToCheck(int id);
        bool SeatExists(int id);
        bool CreateSeat(Seat seat);
        bool UpdateSeat(Seat seat);
        bool DeleteSeat(int id);
        bool Save();
    }
}
