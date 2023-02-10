using System;

namespace Rabobank.TechnicalTest.GCOB.Exceptions
{
    public class AddressExistsException : Exception
    {
        public AddressExistsException(int addressId): base($"Cannot insert address with identity '{addressId}' as it already exists in the collection") { }
    }
}
