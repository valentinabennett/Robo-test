﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rabobank.TechnicalTest.GCOB.Models;
using Rabobank.TechnicalTest.GCOB.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rabobank.TechnicalTest.GCOB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Gets a customer by Id
        /// </summary>
        /// <param name="customerId">The customer Id</param>
        /// <returns>Full details of a customer</returns>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(int customerId)
        {
            _logger.LogDebug($"Getting customer by id {customerId}");

            try
            {
                var customer = await _service.GetCustomerById(customerId);
                if (customer == null)
                {
                    _logger.LogDebug($"Unable to find customer {customerId}");

                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                _logger.LogDebug($"Could not get customer {customerId}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer">the customer id</param>
        /// <returns>Status created</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            _logger.LogDebug("Adding a customer");
            try
            {
               var newCustomer = await _service.AddCustomer(customer);
                return CreatedAtAction(nameof(Get), "Customer", new { customerId = newCustomer.Id }, newCustomer); ;
            }
            catch (Exception)
            {
                _logger.LogDebug("Could not add the customer");
                return BadRequest();
            }
        }

    }
}
