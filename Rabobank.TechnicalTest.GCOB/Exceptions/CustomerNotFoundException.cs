using System;

namespace Rabobank.TechnicalTest.GCOB.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
            public CustomerNotFoundException(int customerId) : base($"Customer {customerId} does not exist")
            {
            }
        }
    }
