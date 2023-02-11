using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface ICustomerService
    {
       Task<Customer> AddCustomer(Customer customer);
        Task<Customer> GetCustomerById(int id);
    }
}
