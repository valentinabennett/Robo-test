using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class AddressServiceTest
    {
        protected Mock<ILogger<AddressService>> MockLogger;
        protected Mock<ICountryService> MockCountryService;
        protected Mock<IAddressRepository> MockAddressRepository;
        protected AddressService service;
        protected AutoMock Mocker;

        [TestInitialize]

        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<AddressService>>();
            MockCountryService = Mocker.Mock<ICountryService>();
            MockAddressRepository= Mocker.Mock<IAddressRepository>();
            service = Mocker.Create<AddressService>();
        }

        [TestMethod]
        public async Task GivenHaveAddress_AndGenerateAddressId_ReturnCreatedId()
        {
            MockAddressRepository.Setup(x => x.GenerateIdentityAsync()).Returns(Task.FromResult(1));

            var result = await service.GenerateIdentityAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            MockAddressRepository.Verify(x => x.GenerateIdentityAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GivenHaveAddress_AndGetAddressById_ReturnAddress()
        {
            var addressDto = GetAddress();
            MockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(addressDto));

            var result = await service.GetAddressById(addressDto.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(addressDto.Id, result.Id);
            MockAddressRepository.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenHaveAddress_AndInserttAddress_AddressAddedToDb()
        {
            var addressDto = GetAddress();
            var customer = GetCustomer();
            MockAddressRepository.Setup(x => x.InsertAsync(It.IsAny<AddressDto>()));

             await service.InsertAddress(1, customer);

            MockAddressRepository.Verify(x => x.InsertAsync(It.IsAny<AddressDto>()), Times.Once);
        }

        private AddressDto GetAddress()
        {
            return new AddressDto { Id = 12, Street = "Street1", City = "City1", CountryId = 1, Postcode = "AAA 2AS" };
        }
        private Customer GetCustomer()
        {
            return new Customer { Id = 1, Street = "Street1", City = "City1", Postcode = "AAA 2AS" };
        }
    }
}
