using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface ICustomerService
    {
       Task AddCustomer(Customer customer);
        Task<Customer> GetCustomerById(int id);
    }
}
