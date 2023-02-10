using Microsoft.Extensions.Logging;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Models;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly ILogger<CountryService> _logger;
        public CountryService(ICountryRepository repository, ILogger<CountryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Country> GetCountryById(int id)
        {
            var countryDto = await _repository.GetAsync(id);
            var country = new Country { Id = countryDto.Id, Name = countryDto.Name };
            return country;
        }

        public async Task<Country> GetCountryByName(string name)
        {
            Country country = null;
            try
            {
                var countries = await _repository.GetAllAsync();
                var countryDto = countries?.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

                if (countryDto != null)
                {
                    country = new Country { Id = countryDto.Id, Name = countryDto.Name };
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Country name {name} is not found, {ex.Message}");
                throw;
            }
           
            return country;
        }
    }
}
