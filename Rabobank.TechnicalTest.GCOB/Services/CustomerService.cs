using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Validations;
using System.Linq;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        public CustomerService(ICustomerRepository repository, IAddressService addressService, ICountryService countryService)
        {
            _repository = repository;
            _addressService = addressService;
            _countryService = countryService;
        }
        public async Task AddCustomer(Customer customer)
        {
            var validator = new CustomerValidation();

            var result = await validator.ValidateAsync(customer);
            if (result.IsValid)
            {
                var id = await _repository.GenerateIdentityAsync();

                var addressId = await _addressService.GenerateIdentityAsync();

                await _addressService.InsertAddress(addressId, customer);

                var names = customer.FullName.Split(' ');
                var customerDto = new CustomerDto
                {
                    Id = id,
                    FirstName = names.Length > 0 ? names[0] : string.Empty,
                    LastName = names.Length > 1 ? names[1] : string.Empty,
                    AddressId = addressId,
                };

                await _repository.InsertAsync(customerDto);
            }

        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customerDto = await _repository.GetAsync(id);
            Customer customer = null;
            if (customerDto != null)
            {
                var address = customerDto.AddressId > 0 ? await _addressService.GetAddressById(customerDto.AddressId) : null;
                var country = address != null ? await _countryService.GetCountryById(address.CountryId) : null;

                customer = new Customer
                {
                    Id = customerDto.Id,
                    FullName = $"{customerDto.FirstName} {customerDto.LastName}",
                    City= address?.City,
                    Street= address?.Street,
                    Postcode= address?.Postcode,
                    Country = country?.Name
                };

            }

            return customer;
        }
    }
}
