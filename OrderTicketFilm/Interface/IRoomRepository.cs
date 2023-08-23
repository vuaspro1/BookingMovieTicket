using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoomRepository
    {
        PaginationDTO<RoomView> GetRooms(int page, int pageSize);
        ICollection<Room> GetRoomsToCheck();
        RoomView GetRoom(int id);
        RoomView GetRoomByShowTimeId(int showTimeId);
        Room GetRoomToCheck(int roomId);
        ICollection<SeatView> GetSeatsByARoom(int roomId);
        PaginationDTO<ShowTimeView> GetShowTimesByARoom(int roomId, int page, int pageSize);
        bool RoomExists(int id);
        bool CreateRoom(Room room);
        bool UpdateRoom(Room room);
        bool DeleteRoom(int id);
        bool Save();
    }
}
