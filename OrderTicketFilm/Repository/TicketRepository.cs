using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System.Drawing.Printing;

namespace OrderTicketFilm.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public static int PAGE_SIZE { get; set; } = 10;

        public TicketRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateTicket(Ticket ticket)
        {
            _context.Add(ticket);
            return Save();
        }

        public bool DeleteTicket(int id)
        {
            var deleteTicket = _context.Tickets.SingleOrDefault(item => item.Id == id);
            if (deleteTicket != null)
            {
                _context.Tickets.Remove(deleteTicket);
            }
            return Save();
        }

        public TicketView GetTicket(int id)
        {
            var ticket = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime)
                .ThenInclude(item => item.Film).Include(item => item.Seat)
                .ThenInclude(item => item.Room).Where(item => item.Id == id).FirstOrDefault();
            return new TicketView
            {
                Id = ticket.Id,
                SeatId = ticket.Seat.Id,
                ShowTimeId = ticket.ShowTime.Id,
                BillId = ticket.Bill.Id,
                ShowTime = ticket.ShowTime.Time,
                SeatName = ticket.Seat.Name,
                FilmName = ticket.ShowTime.Film.Name,
                RoomName = ticket.ShowTime.Room.Name,
                Price = ticket.Seat.Price
            };
        }

        public ICollection<TicketView> GetTickets(int page)
        {
            var querys = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime)
                .ThenInclude(item => item.Film).Include(item => item.Seat)
                .ThenInclude(item => item.Room).ToList();
            var query = querys.Select(item => new TicketView
            {
                Id = item.Id,
                SeatId = item.Seat.Id,
                ShowTimeId = item.ShowTime.Id,
                BillId = item.Bill.Id,
                ShowTime = item.ShowTime.Time,
                SeatName = item.Seat.Name,
                FilmName = item.ShowTime.Film.Name,
                RoomName = item.ShowTime.Room.Name,
                Price = item.Seat.Price
            }).ToList();
            var result = PaginatedList<TicketView>.Create(query.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TicketExists(int id)
        {
            return _context.Tickets.Any(f => f.Id == id);
        }

        public bool UpdateTicket(Ticket ticket)
        {
            _context.Update(ticket);
            return Save();
        }
    }
}
