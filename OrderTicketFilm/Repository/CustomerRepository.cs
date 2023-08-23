using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System;

namespace OrderTicketFilm.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public CustomerRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateCustomer(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }

        public bool DeleteCustomer(int id)
        {
            var deleteCustomer = _context.Customers.SingleOrDefault(item => item.Id == id);
            var deleteBill = _context.Bills.Include(item => item.Customer).FirstOrDefault(item => item.Customer.Id == id);
            if (deleteCustomer != null)
            {
                _context.Customers.Remove(deleteCustomer);
                if ( deleteBill != null)
                {
                    _context.Bills.Remove(deleteBill);
                }
            }
            return Save();
        }

        public CustomerView GetCustomer(int id)
        {
            var customer = _context.Customers.Where(item => item.Id == id).FirstOrDefault();
            return new CustomerView
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
            };
        }

        public PaginationDTO<CustomerView> GetCustomerBySearch(string search, int page, int pageSize)
        {
            PaginationDTO<CustomerView> pagination = new PaginationDTO<CustomerView>();
            //return _context.Customers.Where(item => item.Name == name).FirstOrDefault();
            var query = _context.Customers.Where(item => item.Name.Trim().ToUpper().Contains(search.TrimEnd().ToUpper())
            || item.Address.Trim().ToUpper().Contains(search.TrimEnd().ToUpper()));

            var customer = query.Select(item => new CustomerView
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
            }).ToList();
            var result = PaginatedList<CustomerView>.Create(customer.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = customer.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public PaginationDTO<CustomerView> GetCustomers(int page, int pageSize)
        {
            PaginationDTO<CustomerView> pagination = new PaginationDTO<CustomerView>();
            var customers = _context.Customers.OrderBy(c => c.Id);
            var customer = customers.Select(item => new CustomerView
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
            }).ToList();
            var result = PaginatedList<CustomerView>.Create(customer.AsQueryable(), page, pageSize);

            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = customers.Count();
            pagination.pageSize = pageSize;
            return pagination;

        }

        public PaginationDTO<BillView> GetBillByACustomer(int customerId, int page, int pageSize)
        {
            PaginationDTO<BillView> pagination = new PaginationDTO<BillView>();
            var bills = _context.Bills.Include(item => item.Customer).Include(item => item.User)
                .Where(r => r.Customer.Id == customerId).ToList();
            var bill = bills.Select(item => new BillView
            {
                Id = item.Id,
                CreateDate = item.CreateDate,
                CustomerId = item.Customer.Id,
                UserId = item.User.Id ,
                CustomerName = item.Customer.Name  ,
                UserName = item.User.Name ,
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

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }

        public Customer GetCustomerToCheck(int id)
        {
            var customer = _context.Customers.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<Customer>(customer);
        }

        public ICollection<Customer> GetCustomersToCheck()
        {
            var customers = _context.Customers.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<Customer>>(customers);
        }
    }
}
