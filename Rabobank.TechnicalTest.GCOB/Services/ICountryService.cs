using Rabobank.TechnicalTest.GCOB.Models;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public interface ICountryService
    { 
        Task<Country> GetCountryById(int id);
        Task<Country> GetCountryByName(string name);

    }
}
