using System.Threading.Tasks;
using Rabobank.TechnicalTest.GCOB.Models;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface ICustomerService
    {
       Task<Customer> AddCustomer(Customer customer);
        Task<Customer> GetCustomerById(int id);
    }
}
