using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IBillRepository
    {
        ICollection<BillView> GetBills(int page);
        BillView GetBill(int id);
        Bill GetBillToCheck(int id);
        ICollection<TicketView> GetTicketsByABill(int id, int page);
        bool BillExists(int id);
        bool CreateBill(Bill bill);
        bool DeleteBill(int id);
        bool UpdateBill(Bill bill);
        bool Save();
    }
}
