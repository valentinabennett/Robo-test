using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System.Threading.Tasks;


namespace Rabobank.TechnicalTest.GCOB.Tests.Repositories
{
    [TestClass]
    public class AddressRepositoryTest
    {
        protected Mock<ILogger<InMemoryAddressRepository>> MockLogger;
        protected AutoMock Mocker;
        protected InMemoryAddressRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<InMemoryAddressRepository>>();
            repository = Mocker.Create<InMemoryAddressRepository>();
        }

        [TestMethod]
        public async Task GivenHaveAnAddress_AndIGetNoRecordsInDB_ThenTheIdentityNumberCreated()
        {
            var result = await repository.GenerateIdentityAsync();
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task GivenHaveAnAddress_AndIGetAddressesInDB_ThenTheIdentityNumberCreated()
        {
            var addressNew = GetAddress();
            await repository.InsertAsync(addressNew);

            var result = await repository.GenerateIdentityAsync();
            Assert.IsTrue(result == ++addressNew.Id);
        }

        [TestMethod, ExpectedException(typeof(AddressExistsException))]
        public async Task GivenHaveAnAddressExistsInDB_AndIGetTheSameAddress_ThenTheExceptionIsThrown()
        {
            var addressNew = GetAddress();
            await repository.InsertAsync(addressNew);

            await repository.InsertAsync(addressNew);
        }

        [TestMethod]
        public async Task GivenHaveAnAddress_AndIGetANewAddress_ThenTheAddressIsInserted()
        {
            var addressNew = GetAddress();
            await repository.InsertAsync(addressNew);

            var result = await repository.GetAsync(addressNew.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(addressNew.Id, result.Id);
            Assert.AreEqual(addressNew.Street, result.Street);
            Assert.AreEqual(addressNew.City, result.City);
            Assert.AreEqual(addressNew.CountryId, result.CountryId);
            Assert.AreEqual(addressNew.Postcode, result.Postcode);

        }

        [TestMethod]
        public async Task GivenHaveAnAddress_AndIGetTheAddresssFromTheDB_ThenTheAddressIsRetrieved()
        {
            var addressNew = GetAddress();
            await repository.InsertAsync(addressNew);

            var result = await repository.GetAsync(addressNew.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(addressNew.Id, result.Id);
            Assert.AreEqual(addressNew.Street, result.Street);
            Assert.AreEqual(addressNew.City, result.City);
            Assert.AreEqual(addressNew.CountryId, result.CountryId);
            Assert.AreEqual(addressNew.Postcode, result.Postcode);
        }


        [TestMethod, ExpectedException(typeof(AddressNotFoundException))]
        public async Task GivenHaveAnAddressNotExistInDB_AndIGetAddressFromDB_ThenTheExceptionIsThrown()
        {
            var result = await repository.GetAsync(1);
        }

        [TestMethod]
        public async Task GivenHaveAnAddress_AndIUpdatedTheAddressInTheDB_ThenTheAddressIsUpdated()
        {
            var addressNew = GetAddress();
            await repository.InsertAsync(addressNew);

            var addressUpdated = GetAddressUpdate();

            await repository.UpdateAsync(addressUpdated);

            var result = await repository.GetAsync(addressUpdated.Id);
            Assert.AreEqual(addressUpdated.Id, result.Id);
            Assert.AreEqual(addressUpdated.Street, result.Street);
            Assert.AreEqual(addressUpdated.City, result.City);
            Assert.AreEqual(addressUpdated.CountryId, result.CountryId);
            Assert.AreEqual(addressUpdated.Postcode, result.Postcode);
        }

        [TestMethod, ExpectedException(typeof(AddressNotFoundException))]
        public async Task GivenHaveAnAddressNotExistInDB_AndIGetAddressUpdate_ThenTheExceptionIsThrown()
        {
            var addressUpdated = GetAddressUpdate();

            await repository.UpdateAsync(addressUpdated);
        }

        private AddressDto GetAddressUpdate()
        {
            return new AddressDto { Id = 12, Street = "Street2", City = "City2", CountryId = 1, Postcode = "AAA 2AS" };
        }
        private AddressDto GetAddress()
        {
            return new AddressDto { Id = 12, Street = "Street1", City = "City1", CountryId = 1, Postcode = "AAA 2AS" };
        }
    }
}
