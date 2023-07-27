using AutoMapper;
using FluentAssertions.Equivalency;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public BillRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool BillExists(int id)
        {
            return _context.Bills.Any(f => f.Id == id);
        }

        public bool CreateBill(Bill bill)
        {
            _context.Add(bill);
            return Save();
        }

        public bool DeleteBill(int id)
        {
            var deleteBill = _context.Bills.SingleOrDefault(item => item.Id == id);
            if (deleteBill != null)
            {
                _context.Bills.Remove(deleteBill);
            }
            return Save();
        }

        public BillDto GetBill(int id)
        {
            var bill = _context.Bills.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<BillDto>(bill);
        }

        public ICollection<BillDto> GetBills()
        {
            var bills = _context.Bills.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<BillDto>>(bills);
        }

        public Bill GetBillToCheck(int id)
        {
            var bill = _context.Bills.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Bill>(bill);
        }

        public ICollection<TicketDto> GetTicketsByABill(int id)
        {
            var query = _context.Tickets.Where(e => e.Bill.Id == id).ToList();
            return _mapper.Map<List<TicketDto>>(query);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBill(Bill bill)
        {
            _context.Update(bill);
            return Save();
        }
    }
}
