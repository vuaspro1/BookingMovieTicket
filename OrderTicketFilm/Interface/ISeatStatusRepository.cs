using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ISeatStatusRepository
    {
        PaginationDTO<SeatStatusView> GetSeatStatuses(int page, int pageSize);
        ICollection<SeatStatusDto> GetSeatStatusesToCheck();
        SeatStatusView GetStatus(int id);
        SeatStatus GetStatusToCheck(int id);
        bool SeatStatusExists(int id);
        bool CreateSeatStatus(SeatStatus seatStatus);
        bool DeleteSeatStatus(int id);
        bool UpdateSeatStatus(SeatStatus seatStatus);
        bool Save();
    }
}
