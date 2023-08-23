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
            var deleteSeat = _context.Seats.FirstOrDefault(item => item.Id == id);
            if (deleteSeat != null)
            {
                _context.Seats.Remove(deleteSeat);
            }
            return Save();
        }

        public SeatView GetSeat(int id)
        {
            var seat = _context.Seats.Where(item => item.Id == id).FirstOrDefault();
            return new SeatView
            {
                Id = seat != null ? seat.Id : 0,
                Name = seat != null ? seat.Name : "Unknown",
                Price = seat != null ? seat.Price : 0,
                RoomId = seat != null ? seat.Room.Id : 0,
                Row = seat != null ? seat.Row : 0,
                Column = seat != null ? seat.Column : 0,
            };
        }

        public PaginationDTO<SeatView> GetSeats(int page, int pageSize)
        {
            PaginationDTO<SeatView> pagination = new PaginationDTO<SeatView>();
            var seats = _context.Seats.Include(item => item.Room)
                .OrderBy(c => c.Id).ToList();
            var seat = seats.Select(item => new SeatView
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                RoomId = item.Room.Id,
                Row = item.Row,
                Column = item.Column,
            }).ToList();
            var result = PaginatedList<SeatView>.Create(seat.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = seat.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public ICollection<SeatDto> GetSeatsToCheck()
        {
            var seats = _context.Seats.ToList();
            return _mapper.Map<List<SeatDto>>(seats);
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
            return _context.Seats.Any(c => c.Id == id);
        }

        public bool UpdateSeat(Seat seat)
        {
            _context.Update(seat);
            return Save();
        }
    }
}
