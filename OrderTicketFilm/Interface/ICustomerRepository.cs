using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Interface
{
    public interface ICustomerRepository
    {
        ICollection<CustomerDto> GetCustomers();
        CustomerDto GetCustomer(int id);
        Customer GetCustomerToCheck(int id);
        List<CustomerDto> GetCustomerByName(string name);
        ICollection<BillDto> GetBillByACustomer(int customerId);
        bool CustomerExists(int id);
        bool CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        bool Save();
    }
}
