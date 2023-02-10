using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Exceptions;
using Rabobank.TechnicalTest.GCOB.Repositories;
using Rabobank.TechnicalTest.GCOB.Services;
using System;
using System.Collections.Generic;
using Rabobank.TechnicalTest.GCOB.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Rabobank.TechnicalTest.GCOB.Tests.Services
{
    [TestClass]
    public class CustomerControllerTest
    {
        protected Mock<ILogger<CustomerController>> MockLogger;
        protected Mock<ICustomerService> MockCustomerService;
        protected CustomerController controller;
        protected AutoMock Mocker;

        [TestInitialize]

        public void Initialize()
        {
            Mocker = AutoMock.GetLoose();
            MockLogger = Mocker.Mock<ILogger<CustomerController>>();
            MockCustomerService = Mocker.Mock<ICustomerService>();
            controller = Mocker.Create<CustomerController>();
        }


        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToInsertTheCustomer_ThenTheCustomerIsInserted()
        {
            var customer = new Customer();
            MockCustomerService.Setup(x => x.AddCustomer(It.IsAny<Customer>()));           
           
            var result = await controller.Post(customer);

            var typedResult = (NoContentResult)result;
             Assert.AreEqual((int)HttpStatusCode.NoContent, typedResult.StatusCode);
            MockCustomerService.Verify(c => c.AddCustomer(It.IsAny<Customer>()), Times.Once);

        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToInsertTheCustomer_ThenReturnsBadRequest()
        {
            var customer = new Customer();
            MockCustomerService.Setup(x => x.AddCustomer(It.IsAny<Customer>())).Throws(new CustomerExistsException(1));

            var result = await controller.Post(customer);

            var typedResult = (BadRequestResult)result;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, typedResult.StatusCode);
            MockCustomerService.Verify(c => c.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsReturned()
        {
            var customer = new Customer { Id = 1, FullName = "Jhon Smith", City = "Goa", Street = "Green", Postcode = "AAA 5AA", Country = "India" };
            MockCustomerService.Setup(x => x.GetCustomerById(It.IsAny<int>())).ReturnsAsync(customer);

            var result = await controller.Get(customer.Id);

            var typedResult = (OkObjectResult)result;
            Assert.AreEqual((int)HttpStatusCode.OK, typedResult.StatusCode);
            MockCustomerService.Verify(c => c.GetCustomerById(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(typedResult.Value);
            var customerResult = typedResult.Value as Customer;
            Assert.IsNotNull(customerResult);
            Assert.AreEqual(customer.Id, customerResult.Id);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ThenTheCustomerIsNotFound()     {
            Customer customer =null;
            MockCustomerService.Setup(x => x.GetCustomerById(It.IsAny<int>())).ReturnsAsync(customer);

            var result = await controller.Get(12);

            var typedResult = (NotFoundResult)result;
            Assert.AreEqual((int)HttpStatusCode.NotFound, typedResult.StatusCode);
            MockCustomerService.Verify(c => c.GetCustomerById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenHaveACustomer_AndICallAServiceToGetTheCustomer_ReturnsBadReslt()
        {
            MockCustomerService.Setup(x => x.GetCustomerById(It.IsAny<int>())).Throws(new CustomerExistsException(12));

            var result = await controller.Get(12);

            var typedResult = (BadRequestResult)result;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, typedResult.StatusCode);
            MockCustomerService.Verify(c => c.GetCustomerById(It.IsAny<int>()), Times.Once);
        }
    }
}