using Microsoft.Extensions.Logging;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Repositories
{
    public class InMemoryAddressRepository : IAddressRepository
    {
        private ConcurrentDictionary<int, AddressDto> Addresses { get; } = new ConcurrentDictionary<int, AddressDto>();
        private ILogger<InMemoryAddressRepository> _logger;

        public InMemoryAddressRepository(ILogger<InMemoryAddressRepository> logger)
        {
            _logger = logger;
        }
        public Task<int> GenerateIdentityAsync()
        {
            _logger.LogDebug("Generating Address identity");
            return Task.Run(() =>
            {
                if (Addresses.Count == 0) return 1;

                var x = Addresses.Values.Max(c => c.Id);
                return ++x;
            });
        }

        public Task<AddressDto> GetAsync(int identity)
        {
            _logger.LogDebug($"Get Address with identity {identity}");

            if (!Addresses.ContainsKey(identity)) throw new AddressNotFoundException(identity);
            _logger.LogDebug($"Found Address with identity {identity}");
            return Task.FromResult(Addresses[identity]);
        }

        public Task InsertAsync(AddressDto address)
        {
            if (Addresses.ContainsKey(address.Id))
            {
                throw new AddressExistsException(address.Id);
            }

            Addresses.TryAdd(address.Id, address);
            _logger.LogDebug($"New address inserted [ID:{address.Id}]. " +
                          $"There are now {Addresses.Count} legal entities in the store.");
            return Task.FromResult(address);
        }

        public Task UpdateAsync(AddressDto address)
        {
            if (!Addresses.ContainsKey(address.Id))
            {
                throw new AddressNotFoundException(address.Id);
            }

            Addresses[address.Id] = address;
            _logger.LogDebug($"New address updated [ID:{address.Id}].");

            return Task.FromResult(address);
        }

    }
}
