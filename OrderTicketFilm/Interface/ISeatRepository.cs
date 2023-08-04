using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ISeatRepository
    {
        ICollection<SeatDto> GetSeats();
        SeatDto GetSeat(int id);
        Seat GetSeatToCheck(int id);
        bool SeatExists(int id);
        bool CreateSeat(Seat seat);
        bool UpdateSeat(Seat seat);
        bool DeleteSeat(int id);
        bool Save();
    }
}
