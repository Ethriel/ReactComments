using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class PersonSignUpValidator : AbstractValidator<PersonSignUp>
    {
        public PersonSignUpValidator()
        {
            RuleFor(p => p.UserName)
                .NotNullOrEmptyString()
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("User name must contain only English letters, numbers or underscores!")
                .Length(5, 20).WithMessage("User name must be between 5 and 20 characters!");

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
