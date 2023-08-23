using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System.Drawing.Printing;

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

        public RoomView GetRoom(int id)
        {
            var room = _context.Rooms.Where(item => item.Id == id).FirstOrDefault();
            return new RoomView
            {
                Id = room.Id,
                Name = room.Name,
            };
        }

        public PaginationDTO<RoomView> GetRooms(int page, int pageSize)
        {
            PaginationDTO<RoomView> pagination = new PaginationDTO<RoomView>();
            var rooms = _context.Rooms.OrderBy(c => c.Id).ToList();
            var room = rooms.Select(item => new RoomView
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();
            var result = PaginatedList<RoomView>.Create(room.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = room.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public Room GetRoomToCheck(int roomId)
        {
            var room = _context.Rooms.Where(item => item.Id == roomId).FirstOrDefault();
            return _mapper.Map<Room>(room);
        }

        public ICollection<SeatView> GetSeatsByARoom(int roomId)
        {
            var seat = _context.Seats.Include(item => item.Room)
                .Where(r => r.Room.Id == roomId).OrderBy(item => item.Column)
                .ThenBy(item => item.Row).ToList();
            return seat.Select(item => new SeatView
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                RoomId = item.Room.Id,
                Row = item.Row,
                Column = item.Column,
            }).ToList();
        }

        public PaginationDTO<ShowTimeView> GetShowTimesByARoom(int roomId, int page, int pageSize)
        {
            PaginationDTO<ShowTimeView> pagination = new PaginationDTO<ShowTimeView>();
            var showTimes = _context.ShowTimes.Include(item => item.Film).ThenInclude(s => s.TypeOfFilm)
                .Include(item => item.Room).Where(item => item.Room.Id == roomId).OrderBy(item => item.Time.Date).ToList();
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
                TypeName = item.Film.TypeOfFilm.Name,
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = showTime.Count();
            pagination.pageSize = pageSize;
            return pagination;
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

        public ICollection<Room> GetRoomsToCheck()
        {
            var room = _context.Rooms.ToList();
            return _mapper.Map<List<Room>>(room);
        }

        public RoomView GetRoomByShowTimeId(int showTimeId)
        {
            var room = _context.Rooms.Include(item => item.ShowTimes)
                .Where(item => item.ShowTimes.Any(item => item.Id == showTimeId)).FirstOrDefault();
            return  new RoomView
            {
                Id = room.Id,
                Name = room.Name,
            };
        }
    }
}
