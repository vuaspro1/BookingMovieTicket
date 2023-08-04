using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ICustomerRepository
    {
        ICollection<CustomerDto> GetCustomers(int page);
        ICollection<Customer> GetCustomersToCheck();
        CustomerDto GetCustomer(int id);
        Customer GetCustomerToCheck(int id);
        ICollection<CustomerDto> GetCustomerByName(string name, int page);
        ICollection<BillView> GetBillByACustomer(int customerId, int page);
        bool CustomerExists(int id);
        bool CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        bool Save();
    }
}
