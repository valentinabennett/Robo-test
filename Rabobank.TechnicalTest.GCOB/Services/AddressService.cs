using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Models;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly ICountryService _countryService;
        public AddressService(IAddressRepository repository, ICountryService countryService)
        {
            _repository = repository;
            _countryService = countryService;
        }

        public async Task<int> GenerateIdentityAsync()
        {
            return await _repository.GenerateIdentityAsync();
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            var addressDto = await _repository.GetAsync(addressId);
            var address = new Address
            {
                Id = addressDto.Id,
                City = addressDto.City,
                CountryId = addressDto.CountryId,
                Postcode = addressDto.Postcode
            };
            return address;
        }

        public async Task InsertAddress(int addressId, Customer customer)
        {
            var country = await _countryService.GetCountryByName(customer.Country);
            var countryId = country != null ? country.Id : 0;
            var addressDto = new AddressDto
            {
                Id = addressId,
                City = customer.City,
                Postcode = customer.Postcode,
                Street = customer.Street,
                CountryId = countryId
            };
            await _repository.InsertAsync(addressDto);
        }
    }
}
