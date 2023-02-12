using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Repositories
{
    [TestClass]
    public class CountryRepositoryTest
    {
        protected Mock<ILogger<InMemoryCountryRepository>> MockLogger;
        protected AutoMock Mocker;
        protected InMemoryCountryRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<InMemoryCountryRepository>>();
            repository = Mocker.Create<InMemoryCountryRepository>();
        }


        [TestMethod]
        public async Task GivenHaveACountryId_AndIGetCountriesInDB_ThenTheCountryRetrieved()
        {
            var countryId = 2;
            var result = await repository.GetAsync(countryId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, countryId);
        }

        [TestMethod, ExpectedException(typeof(CountryNotFoundException))]
        public async Task GivenHaveACountriesInDB_AndIGetTheNotExistingCountryId_ThenTheExceptionIsThrown()
        {
            var countryId = 200;
            var result = await repository.GetAsync(countryId);
        }

        [TestMethod]
        public async Task GivenHaveCountries_AndIGetAllCountriesInDB_ThenTheCountriesRetrieved()
        {
            var result = await repository.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }
    }
}
