using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IBillRepository
    {
        PaginationDTO<BillView> GetBills(int page, int pageSize);
        BillView GetBill(int id);
        Bill GetBillToCheck(int id);
        PaginationDTO<TicketView> GetTicketsByABill(int id, int page, int pageSize);
        bool BillExists(int id);
        bool CreateBill(Bill bill);
        bool DeleteBill(int id);
        bool Save();
    }
}
