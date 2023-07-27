using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public RoomRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateRoom(Room room)
        {
            _context.Add(room);
            return Save();
        }

        public bool DeleteRoom(int id)
        {
            var deleteRoom = _context.Rooms.SingleOrDefault(item => item.Id == id);
            if (deleteRoom != null)
            {
                _context.Rooms.Remove(deleteRoom);
            }
            return Save();
        }

        public RoomDto GetRoom(int id)
        {
            var room = _context.Rooms.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<RoomDto>(room);
        }

        public ICollection<RoomDto> GetRooms()
        {
            var room = _context.Rooms.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<RoomDto>>(room);
        }

        public Room GetRoomToCheck(int roomId)
        {
            var room = _context.Rooms.Where(item => item.Id == roomId).FirstOrDefault();
            return _mapper.Map<Room>(room);
        }

        public ICollection<SeatDto> GetSeatsByARoom(int roomId)
        {
            var seat = _context.Seats.Where(r => r.Room.Id == roomId).ToList();
            return _mapper.Map<List<SeatDto>>(seat);
        }

        public ICollection<ShowTimeDto> GetShowTimesByARoom(int roomId)
        {
            var showTime = _context.ShowTimes.Where(r => r.Room.Id == roomId).ToList();
            return _mapper.Map<List<ShowTimeDto>>(showTime);
        }

        public bool RoomExists(int id)
        {
            return _context.Rooms.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRoom(Room room)
        {
            _context.Update(room);
            return Save();
        }
    }
}
