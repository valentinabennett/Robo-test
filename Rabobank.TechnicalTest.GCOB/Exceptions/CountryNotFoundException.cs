using System;

namespace Rabobank.TechnicalTest.GCOB.Exceptions
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException(int countryId) : base($"Country {countryId} does not exist") { }

    }
}
