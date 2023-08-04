using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public static int PAGE_SIZE { get; set; } = 10;

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

        public ICollection<RoomDto> GetRooms(int page)
        {
            var rooms = _context.Rooms.OrderBy(c => c.Id).ToList();
            var room = rooms.Select(item => new RoomDto
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();
            var result = PaginatedList<RoomDto>.Create(room.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public Room GetRoomToCheck(int roomId)
        {
            var room = _context.Rooms.Where(item => item.Id == roomId).FirstOrDefault();
            return _mapper.Map<Room>(room);
        }

        public ICollection<SeatDto> GetSeatsByARoom(int roomId)
        {
            var seat = _context.Seats.Include(item => item.Room).Where(r => r.Room.Id == roomId)
                .OrderBy(item => item.Id).ToList();
            return seat.Select(item => new SeatDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Status = item.Status,
                RoomId = item.Room.Id
            }).ToList();
        }

        public ICollection<ShowTimeView> GetShowTimesByARoom(int roomId, int page)
        {
            var showTimes = _context.ShowTimes.Where(r => r.Room.Id == roomId).OrderBy(r => r.Time.Date).ToList();
            var showTime = showTimes.Select(item => new ShowTimeView
            {
                Id = item.Id,
                RoomId = item.Room.Id,
                FilmId = item.Film.Id,
                Time = item.Time,
                FilmName = item.Film.Name,
                RoomName = item.Room.Name,
                Duration = item.Film.Time,
                Image = item.Film.Image,
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, PAGE_SIZE);
            return result;
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

        public ICollection<RoomDto> GetRoomsToCheck()
        {
            var room = _context.Rooms.ToList();
            return _mapper.Map<List<RoomDto>>(room);
        }
    }
}
