using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ITicketRepository
    {
        ICollection<TicketDto> GetTickets();
        TicketDto GetTicket(int id);
        bool TicketExists(int id);
        bool CreateTicket(Ticket ticket);
        bool UpdateTicket(Ticket ticket);
        bool DeleteTicket(int id);
        bool Save();
    }
}
