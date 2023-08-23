using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ITicketRepository
    {
        PaginationDTO<TicketView> GetTickets(int page, int pageSize);
        ICollection<Ticket> GetTicketsToCheck();
        ICollection<TicketView> GetTicketsByShowTime(int showTimeId);
        TicketView GetTicket(int id);
        bool TicketExists(int id);
        bool Save();
    }
}
