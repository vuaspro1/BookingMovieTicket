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

        public List<CustomerDto> GetCustomerByName(string? name)
        {
            //return _context.Customers.Where(item => item.Name == name).FirstOrDefault();
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }
            return _mapper.Map<List<CustomerDto>>(query);
        }

        public ICollection<CustomerDto> GetCustomers()
        {
            var customers = _context.Customers.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public ICollection<BillDto> GetBillByACustomer(int customerId)
        {
            var bill = _context.Bills.Where(r => r.Customer.Id == customerId).ToList();
            return _mapper.Map<List<BillDto>>(bill);
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
    }
}
