using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public SeatRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateSeat(Seat seat)
        {
            _context.Add(seat);
            return Save();
        }

        public bool DeleteSeat(int id)
        {
            var deleteSeat = _context.Seats.SingleOrDefault(item => item.Id == id);
            if (deleteSeat != null)
            {
                _context.Seats.Remove(deleteSeat);
            }
            return Save();
        }

        public SeatDto GetSeat(int id)
        {
            var seat = _context.Seats.Where(item => item.Id == id).FirstOrDefault();
            return new SeatDto
            {
                Id = seat.Id,
                Name = seat.Name,
                Price = seat.Price,
                Status = seat.Status,
                RoomId = seat.Room.Id
            };
        }

        public ICollection<SeatDto> GetSeats()
        {
            var seat = _context.Seats.Include(item => item.Room).OrderBy(c => c.Id).ToList();
            return seat.Select(item => new SeatDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Status = item.Status,
                RoomId = item.Room.Id
            }).ToList();
        }

        public Seat GetSeatToCheck(int id)
        {
            var seat = _context.Seats.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Seat>(seat);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SeatExists(int id)
        {
            return _context.TypeOfFilms.Any(c => c.Id == id);
        }

        public bool UpdateSeat(Seat seat)
        {
            _context.Update(seat);
            return Save();
        }
    }
}
