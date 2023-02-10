using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CountryServiceTest
    {
        protected Mock<ILogger<CountryService>> MockLogger;
        protected Mock<ICountryRepository> MockCountryRepository;
        protected CountryService service;
        protected AutoMock Mocker;

        [TestInitialize] 

        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<CountryService>>();
            MockCountryRepository = Mocker.Mock<ICountryRepository>();
            service = Mocker.Create<CountryService>();
        }

        [TestMethod]
        public async Task GivenHaveACountryName_AndICallAServiceToGetTheCountrt_ThenTheCountryIsReturned()
        {
            IEnumerable<CountryDto> countries = new List<CountryDto> { new CountryDto { Id = 1, Name = "England" }, new CountryDto { Id = 2, Name = "Poland" } };

            MockCountryRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(countries));

            var result = await service.GetCountryByName("England");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "England");
        }

        [TestMethod]
        public async Task GivenHaveACountryName_AndICallAServiceToGetTheCountrt_ThenTheCountryIsNotFound_ReturnsNull()
        {
            IEnumerable<CountryDto> countries = null;

            MockCountryRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(countries));

            var result = await service.GetCountryByName("England");

            Assert.IsNull(result);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public async Task GivenHaveACountryName_AndICallAServiceToGetTheCountrt_ThenThrowException()
        {

            MockCountryRepository.Setup(x => x.GetAllAsync()).Throws(new Exception());

            var result = await service.GetCountryByName("England");

        }


        [TestMethod]
        public async Task GivenHaveACountryId_AndICallAServiceToGetTheCountry_ThenTheCountryIsReturned()
        {
            var country =  new CountryDto { Id = 1, Name = "England" };

            MockCountryRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(country));

            var result = await service.GetCountryById(country.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, country.Id);
            Assert.AreEqual(result.Name, country.Name);
        }
    }
}
