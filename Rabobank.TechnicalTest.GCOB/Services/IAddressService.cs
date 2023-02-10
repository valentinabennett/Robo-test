using Rabobank.TechnicalTest.GCOB.Models;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressById(int addressId);

        Task InsertAddress(int addressId, Customer customer);

        Task<int> GenerateIdentityAsync();
    }
}
