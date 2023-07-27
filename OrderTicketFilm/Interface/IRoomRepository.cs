using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IRoomRepository
    {
        ICollection<RoomDto> GetRooms();
        RoomDto GetRoom(int id);
        Room GetRoomToCheck(int roomId);
        ICollection<SeatDto> GetSeatsByARoom(int roomId);
        ICollection<ShowTimeDto> GetShowTimesByARoom(int roomId);
        bool RoomExists(int id);
        bool CreateRoom(Room room);
        bool UpdateRoom(Room room);
        bool DeleteRoom(int id);
        bool Save();
    }
}
