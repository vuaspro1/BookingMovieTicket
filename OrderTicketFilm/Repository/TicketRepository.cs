using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

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
            var deleteTicket = _context.Tickets.SingleOrDefault(item => item.Id == id);
            if (deleteTicket != null)
            {
                _context.Tickets.Remove(deleteTicket);
            }
            return Save();
        }

        public TicketDto GetTicket(int id)
        {
            var ticket = _context.Tickets.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<TicketDto>(ticket);
        }

        public ICollection<TicketDto> GetTickets()
        {
            var tickets = _context.Tickets.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<TicketDto>>(tickets);
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
