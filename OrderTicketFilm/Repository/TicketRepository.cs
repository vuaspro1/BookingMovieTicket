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
            var deleteTicket = _context.Tickets.FirstOrDefault(item => item.Id == id);
            
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
                Price = ticket.Seat.Price,
                RoomId = ticket.ShowTime.Room != null ? ticket.ShowTime.Room.Id : 0,
            };
        }

        public PaginationDTO<TicketView> GetTickets(int page, int pageSize)
        {
            PaginationDTO<TicketView> pagination = new PaginationDTO<TicketView>();
            var querys = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime)
                .ThenInclude(item => item.Film).Include(item => item.Seat)
                .ThenInclude(item => item.Room).ToList();
            var query = querys.Select(item => new TicketView
            {
                Id = item.Id,
                SeatId = item.Seat != null ? item.Seat.Id : 0,
                ShowTimeId = item.ShowTime != null ? item.ShowTime.Id : 0,
                BillId = item.Bill != null ? item.Bill.Id : 0,  
                ShowTime = item.ShowTime != null ? item.ShowTime.Time : DateTime.MinValue,  
                SeatName = item.Seat != null ? item.Seat.Name : string.Empty,  
                FilmName = item.ShowTime != null ? item.ShowTime.Film.Name : string.Empty, 
                RoomName = item.ShowTime != null ? item.ShowTime.Room.Name : string.Empty,  
                Price = item.Seat != null ? item.Seat.Price : 0,
                RoomId = item.ShowTime.Room != null ? item.ShowTime.Room.Id : 0,
            }).ToList();
            var result = PaginatedList<TicketView>.Create(query.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = query.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public ICollection<TicketView> GetTicketsByShowTime(int showTimeId)
        {
            var tickets = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime).Include(item => item.Seat)
                .ThenInclude(item => item.Room).Where(item => item.ShowTime.Id == showTimeId).ToList();
            return tickets.Select(item => new TicketView
            {
                Id = item.Id,
                BillId = item.Bill.Id,
                SeatId = item.Seat != null ? item.Seat.Id : 0,
                ShowTimeId = item.ShowTime != null ? item.ShowTime.Id : 0,
                RoomId = item.ShowTime.Room != null ? item.ShowTime.Room.Id : 0,
            }).ToList();
        }

        public ICollection<Ticket> GetTicketsToCheck()
        {
            var ticket = _context.Tickets.ToList();
            return _mapper.Map<List<Ticket>>(ticket);
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
