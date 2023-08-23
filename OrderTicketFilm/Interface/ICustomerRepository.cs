using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ICustomerRepository
    {
        PaginationDTO<CustomerView> GetCustomers(int page, int pageSize);
        ICollection<Customer> GetCustomersToCheck();
        CustomerView GetCustomer(int id);
        Customer GetCustomerToCheck(int id);
        PaginationDTO<CustomerView> GetCustomerBySearch(string search, int page, int pageSize);
        PaginationDTO<BillView> GetBillByACustomer(int customerId, int page, int pageSize);
        bool CustomerExists(int id);
        bool CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        bool Save();
    }
}
