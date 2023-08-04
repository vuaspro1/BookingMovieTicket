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
        public static int PAGE_SIZE {  get; set; } = 10;

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
            if (deleteCustomer != null)
            {
                _context.Customers.Remove(deleteCustomer);
            }
            return Save();
        }

        public CustomerDto GetCustomer(int id)
        {
            var customer = _context.Customers.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<CustomerDto>(customer);
        }

        public ICollection<CustomerDto> GetCustomerByName(string? name, int page)
        {
            //return _context.Customers.Where(item => item.Name == name).FirstOrDefault();
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }
            var customer = query.Select(item => new CustomerDto
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
            }).ToList();
            var result = PaginatedList<CustomerDto>.Create(customer.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public ICollection<CustomerDto> GetCustomers(int page)
        {
            var customers = _context.Customers.OrderBy(c => c.Id);
            var customer = customers.Select(item => new CustomerDto
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                DateOfBirth = item.DateOfBirth,
            }).ToList();
            var result = PaginatedList<CustomerDto>.Create(customer.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public ICollection<BillView> GetBillByACustomer(int customerId, int page)
        {
            var bills = _context.Bills.Where(r => r.Customer.Id == customerId).ToList();
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
            var customers = _context.Customers.OrderBy(c => c.Id);
            return _mapper.Map<List<Customer>>(customers);
        }
    }
}
