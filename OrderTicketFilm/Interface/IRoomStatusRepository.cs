using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoomStatusRepository
    {
        ICollection<RoomStatusDto> GetRoomStatuses();
        RoomStatus GetStatus(int id);
        bool RoomStatusExists(int id);
        bool CreateRoomStatus(RoomStatus roomStatus);
        bool DeleteRoomStatus(int id);
        bool UpdateRoomStatus(RoomStatus roomStatus);
        bool Save();
    }
}
