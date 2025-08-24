using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class PersonAuthValidator : AbstractValidator<PersonAuth>
    {
        public PersonAuthValidator()
        {
            RuleFor(pa => pa.Email)
                .NotNullOrEmptyString()
                .MustBeValidEmail();

            RuleFor(pa => pa.Password)
                .NotNullOrEmptyString()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MustBeValidPassword();
        }
    }
}
