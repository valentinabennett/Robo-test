using FluentValidation;
using Rabobank.TechnicalTest.GCOB.Models;

namespace Rabobank.TechnicalTest.GCOB.Validations
{
    public class CustomerValidation: AbstractValidator<Customer>
    {
        public const string NoFullNameError = "Full name is required";
        public const string NoStreetError = "Street is required";
        public const string NoCityError = "City is required";
        public const string NoPostcodeError = "Postcode is required";
        public const string NoCountryError = "Country is required";


        public CustomerValidation() {
            RuleFor(x => x.FullName).NotEmpty().WithMessage(NoFullNameError);
            RuleFor(x => x.Street).NotEmpty().WithMessage(NoStreetError);
            RuleFor(x => x.City).NotEmpty().WithMessage(NoCityError);
            RuleFor(x => x.Postcode).NotEmpty().WithMessage(NoPostcodeError);
            RuleFor(x => x.Country).NotEmpty().WithMessage(NoCountryError);

        }
    }
}

