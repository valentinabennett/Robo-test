using System;

namespace Rabobank.TechnicalTest.GCOB.Exceptions
{
    public class AddressNotFoundException: Exception
    {   
        public AddressNotFoundException(int addressId): base($"Address {addressId} does not exist")   { }
    }
}
