using System;

namespace Rabobank.TechnicalTest.GCOB.Exceptions
{
    public class CustomerExistsException : Exception
    {
        public CustomerExistsException(int customerId) : base($"Cannot insert customer with identity '{customerId}' as it already exists in the collection")
        {
        }
    }
}
