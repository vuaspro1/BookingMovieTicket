using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface IBillRepository
    {
        ICollection<BillDto> GetBills();
        BillDto GetBill(int id);
        Bill GetBillToCheck(int id);
        ICollection<TicketDto> GetTicketsByABill(int id);
        bool BillExists(int id);
        bool CreateBill(Bill bill);
        bool DeleteBill(int id);
        bool UpdateBill(Bill bill);
        bool Save();
    }
}
