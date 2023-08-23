using AutoMapper;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System.Net.Sockets;

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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Bills.Add(bill);
                    _context.SaveChanges();
                    var priceTotal = 0;

                    foreach (Ticket itemticket in bill.Tickets.ToList())
                    {
                        var seatPrice = _context.Seats
                            .Where(item => item.Id == itemticket.Seat.Id)
                            .Select(item => item.Price)
                            .FirstOrDefault();

                        if (seatPrice > 0)
                        {
                            priceTotal += seatPrice;
                        }
                        //bill.Tickets.Add(itemticket);
                        //itemticket.Bill = bill; // Thay đổi này để gán hóa đơn cho vé
                    }
                    
                 
                    bill.Quantity = bill.Tickets.Count();
                    bill.PriceTotal = priceTotal;
                    _context.Bills.Update(bill);


                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception) { 
                    
                    transaction.Rollback();
                    // Xử lý lỗi tại đây nếu cần
                    return false;
                }
            }
            return true;
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
                CustomerId = bill.Customer != null ? bill.Customer.Id : 0,
                UserId = bill.User.Id,
                CustomerName = bill.Customer != null ? bill.Customer.Name : "Unknown",
                UserName = bill.User.Name,
                PriceTotal = bill.PriceTotal,
                Quantity = bill.Quantity,
            };
        }

        public PaginationDTO<BillView> GetBills(int page, int pageSize)
        {
            PaginationDTO<BillView> pagination = new PaginationDTO<BillView>();
            var bills = _context.Bills.Include(item => item.Customer).Include(item => item.User)
                .OrderBy(c => c.Id).ToList();
            var bill = bills.Select(item => new BillView
            {
                Id = item.Id,
                CreateDate = item.CreateDate,
                CustomerId = item.Customer != null ? item.Customer.Id : 0,
                UserId = item.User.Id,
                CustomerName = item.Customer != null ? item.Customer.Name : "Unknown",
                UserName = item.User.Name,
                PriceTotal = item.PriceTotal,
                Quantity = item.Quantity,
            }).ToList();
            var result = PaginatedList<BillView>.Create(bill.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = bill.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public Bill GetBillToCheck(int id)
        {
            var bill = _context.Bills.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Bill>(bill);
        }

        public PaginationDTO<TicketView> GetTicketsByABill(int id, int page, int pageSize)
        {
            PaginationDTO<TicketView> pagination = new PaginationDTO<TicketView>();
            var querys = _context.Tickets.Include(item => item.Bill).Include(item => item.ShowTime)
                .ThenInclude(item => item.Film).Include(item => item.Seat)
                .ThenInclude(item => item.Room).Where(e => e.Bill.Id == id).ToList();
            var query = querys.Select(item => new TicketView
            {
                Id = item.Id,
                SeatId = item.Seat != null ? item.Seat.Id : 0,
                ShowTimeId = item.ShowTime != null ? item.ShowTime.Id : 0,
                BillId = item.Bill != null ? item.Bill.Id : 0,  // Check if Bill is not null
                ShowTime = item.ShowTime != null ? item.ShowTime.Time : DateTime.MinValue,  // Check if ShowTime is not null
                SeatName = item.Seat != null ? item.Seat.Name : string.Empty,  // Check if Seat is not null
                FilmName = item.ShowTime != null ? item.ShowTime.Film.Name : string.Empty,  // Check if ShowTime and Film are not null
                RoomName = item.ShowTime != null ? item.ShowTime.Room.Name : string.Empty,  // Check if ShowTime and Room are not null
                Price = item.Seat != null ? item.Seat.Price : 0
            }).ToList();
            var result = PaginatedList<TicketView>.Create(query.AsQueryable(), page, pageSize);
            pagination.data = result ;
            pagination.page = page;
            pagination.totalItem = query.Count();
            pagination.pageSize = pageSize;
            return pagination;
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
