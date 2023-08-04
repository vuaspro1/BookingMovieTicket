using AutoMapper;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public static int PAGE_SIZE { get; set; } = 10;

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
            var price = _context.Tickets.Where(item => item.Id == bill.Id).Select(f => f.Seat.Price).FirstOrDefault();
            bill.Quantity = _context.Tickets.Count(item => item.Bill.Id == bill.Id);
            bill.PriceTotal = bill.Quantity * price;
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

        public BillView GetBill(int id)
        {
            var bill = _context.Bills.Include(item => item.Customer).Include(item => item.User)
                .Where(item => item.Id == id).FirstOrDefault();
            return new BillView
            {
                Id = bill.Id,
                CreateDate = bill.CreateDate,
                CustomerId = bill.Customer.Id,
                UserId = bill.User.Id,
                CustomerName = bill.Customer.Name,
                UserName = bill.User.Name,
                PriceTotal = bill.PriceTotal,
                Quantity = bill.Quantity,
            };
        }

        public ICollection<BillView> GetBills(int page)
        {
            var bills = _context.Bills.Include(item => item.Customer).Include(item => item.User)
                .OrderBy(c => c.Id).ToList();
            var bill = bills.Select(item => new BillView
            {
                Id = item.Id,
                CreateDate = item.CreateDate,
                CustomerId = item.Customer.Id,
                UserId = item.User.Id,
                CustomerName = item.Customer.Name,
                UserName = item.User.Name,
                PriceTotal = item.PriceTotal,
                Quantity = item.Quantity,
            }).ToList();
            var result = PaginatedList<BillView>.Create(bill.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public Bill GetBillToCheck(int id)
        {
            var bill = _context.Bills.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Bill>(bill);
        }

        public ICollection<TicketView> GetTicketsByABill(int id, int page)
        {
            var querys = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime)
                .ThenInclude(item => item.Film).Include(item => item.Seat)
                .ThenInclude(item => item.Room).Where(e => e.Bill.Id == id).ToList();
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

        public bool UpdateBill(Bill bill)
        {
            var price = _context.Tickets.Where(item => item.Id == bill.Id).Select(f => f.Seat.Price).FirstOrDefault();
            bill.Quantity = _context.Tickets.Count(item => item.Bill.Id == bill.Id);
            bill.PriceTotal = bill.Quantity * price;
            _context.Update(bill);
            return Save();
        }
    }
}
