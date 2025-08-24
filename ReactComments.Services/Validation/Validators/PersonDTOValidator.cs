using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class PersonDTOValidator : AbstractValidator<PersonDTO>
    {
        public PersonDTOValidator()
        {
            RuleFor(p => p.Id)
                .MustBeValidDigit();

            RuleFor(p => p.UserName)
                .NotNullOrEmptyString()
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("User name must contain only English letters, numbers or underscores!")
                .Length(5, 20).WithMessage("User name must be between 5 and 20 characters!");

            RuleFor(p => p.Email)
                .NotNullOrEmptyString()
                .MustBeValidEmail();

            RuleFor(p => p.HomePage)
                .MaximumLength(200).WithMessage("Home page can't exceed 200 characters")
                .Must(value => Uri.TryCreate(value, UriKind.Absolute, out _))
                .WithMessage("Home page must be a valid URL")
                .When(p => !string.IsNullOrWhiteSpace(p.HomePage));

            RuleFor(p => p.Role)
                .NotNullOrEmptyString();

            RuleFor(p => p.RegisteredDate)
                .NotNullOrEmptyString()
                .MustBeValidDate();
        }
    }
}
