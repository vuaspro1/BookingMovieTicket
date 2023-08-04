using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoomRepository
    {
        ICollection<RoomDto> GetRooms(int page);
        ICollection<RoomDto> GetRoomsToCheck();
        RoomDto GetRoom(int id);
        Room GetRoomToCheck(int roomId);
        ICollection<SeatDto> GetSeatsByARoom(int roomId);
        ICollection<ShowTimeView> GetShowTimesByARoom(int roomId, int page);
        bool RoomExists(int id);
        bool CreateRoom(Room room);
        bool UpdateRoom(Room room);
        bool DeleteRoom(int id);
        bool Save();
    }
}
