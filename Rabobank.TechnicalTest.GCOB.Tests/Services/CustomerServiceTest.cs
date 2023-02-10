using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Models;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerServiceTest
    {

        protected Mock<ILogger<CustomerService>> MockLogger;
        protected Mock<ICustomerRepository> MockCustomerRepository;
        protected Mock<IAddressService> MockAddressService;
        protected Mock<ICountryService> MockCountryService;
        protected CustomerService service;
        protected AutoMock Mocker;

        [TestInitialize]

        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<CustomerService>>();
            MockCustomerRepository = Mocker.Mock<ICustomerRepository>();
            MockAddressService = Mocker.Mock<IAddressService>();
            MockCountryService = Mocker.Mock<ICountryService>();
            service = Mocker.Create<CustomerService>();
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
        {
            MockCustomerRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new CustomerDto { Id=1, FirstName="John", LastName="Smith", AddressId=2});

            MockAddressService.Setup(x => x.GetAddressById(It.IsAny<int>()))
                .ReturnsAsync(new Address { Id = 2, City = "Goa", CountryId = 3, Postcode = "AAA 5AA", Street = "Blue" });

            MockCountryService.Setup(x => x.GetCountryById(It.IsAny<int>())).ReturnsAsync(new Country { Id=3, Name="India"});


            var customer = await service.GetCustomerById(1);

            Assert.IsNotNull(customer);
            Assert.AreEqual(1, customer.Id);
            Assert.AreEqual("John Smith", customer.FullName);
            Assert.AreEqual("India", customer.Country);
            Assert.AreEqual("AAA 5AA", customer.Postcode);

        }

        [TestMethod]
        public async Task GivenInsertACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsInserted()
        {
            MockCustomerRepository.Setup(x => x.InsertAsync(It.IsAny<CustomerDto>()));
            MockCustomerRepository.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(1);
            MockAddressService.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(4);

            var customer = new Customer { City = "Goa", Street = "Blus", Country = "India", FullName = "John Smith", Postcode="AAA 5AA" };
            await service.AddCustomer(customer);

            MockCustomerRepository.Verify(x => x.InsertAsync(It.IsAny<CustomerDto>()), Times.Once);
            MockAddressService.Verify(x => x.GenerateIdentityAsync(), Times.Once);
            MockCustomerRepository.Verify(x => x.GenerateIdentityAsync(), Times.Once);
        }
        [TestMethod]
        public async Task GivenInsertACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsNoValid_AndCustomerIsNotInserted()
        {
            MockCustomerRepository.Setup(x => x.InsertAsync(It.IsAny<CustomerDto>()));
            MockCustomerRepository.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(1);
            MockAddressService.Setup(x => x.GenerateIdentityAsync()).ReturnsAsync(4);

            var customer = new Customer { City = "Goa",  FullName = "John Smith", Postcode = "AAA 5AA" };
            await service.AddCustomer(customer);

            MockCustomerRepository.Verify(x => x.InsertAsync(It.IsAny<CustomerDto>()), Times.Never);
            MockAddressService.Verify(x => x.GenerateIdentityAsync(), Times.Never);
            MockCustomerRepository.Verify(x => x.GenerateIdentityAsync(), Times.Never);
        }
    }
}