using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Repositories;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        protected Mock<ILogger<InMemoryCustomerRepository>> MockLogger;
        protected AutoMock Mocker;
        protected InMemoryCustomerRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<InMemoryCustomerRepository>>();
            repository = Mocker.Create<InMemoryCustomerRepository>();
        }


        [TestMethod]
        public async Task GivenHaveACustomer_AndIGetNoCustomersInDB_ThenTheIdentityNumberCreated()
        {
            var result = await repository.GenerateIdentityAsync();
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndIGetCustomersInDB_ThenTheIdentityNumberCreated()
        {
            var customerNew = GetCustomer();
            await repository.InsertAsync(customerNew);

            var result = await repository.GenerateIdentityAsync();
            Assert.IsTrue(result == ++customerNew.Id);
        }

        [TestMethod, ExpectedException(typeof(CustomerExistsException))]
        public async Task GivenHaveACustomerExistsInDB_AndIGetTheSameCustomer_ThenTheExceptionIsThrown()
        {
            var customerNew = GetCustomer();
            await repository.InsertAsync(customerNew);

            await repository.InsertAsync(customerNew);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndIGetANewCustomer_ThenTheCustomerIsInserted()
        {
            var customerNew = GetCustomer();
            await repository.InsertAsync(customerNew);


            var result = await repository.GetAsync(customerNew.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(customerNew.Id, result.Id);
            Assert.AreEqual(customerNew.FirstName, result.FirstName);
            Assert.AreEqual(customerNew.LastName, result.LastName);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndIGetTheCustomerFromTheDB_ThenTheCustomerIsRetrieved()
        {
            var customerNew = GetCustomer();
            await repository.InsertAsync(customerNew);

            var result = await repository.GetAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(customerNew.Id, result.Id);
            Assert.AreEqual(customerNew.FirstName, result.FirstName);
            Assert.AreEqual(customerNew.LastName, result.LastName);
        }

        [TestMethod, ExpectedException(typeof(CustomerNotFoundException))]
        public async Task GivenHaveACustomerNotExistInDB_AndIGetCustomerFromDB_ThenTheExceptionIsThrown()
        {
            var result = await repository.GetAsync(1);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndIUpdatedTheCustomerInTheDB_ThenTheCustomerIsUpdated()
        {
            var customerNew = GetCustomer();
            await repository.InsertAsync(customerNew);

            var customerUpdate = GetCustomerUpdate();

            await repository.UpdateAsync(customerUpdate);

            var result = await repository.GetAsync(customerUpdate.Id);     
            Assert.AreEqual(customerUpdate.Id, result.Id);
            Assert.AreEqual(customerUpdate.FirstName, result.FirstName);
            Assert.AreEqual(customerUpdate.LastName, result.LastName);
        }

        [TestMethod, ExpectedException(typeof(CustomerNotFoundException))]
        public async Task GivenHaveACustomerNotExistInDB_AndIGetCustomerUpdate_ThenTheExceptionIsThrown()
        {
            var customerUpdate = GetCustomerUpdate();

            await repository.UpdateAsync(customerUpdate);
        }

        private CustomerDto GetCustomerUpdate()
        {
            return new CustomerDto { Id = 1, FirstName = "Poul", LastName = "Green" };
        }
        private CustomerDto GetCustomer()
        {
            return new CustomerDto { Id = 1, FirstName = "John", LastName = "Green" };
        }
    }
}
