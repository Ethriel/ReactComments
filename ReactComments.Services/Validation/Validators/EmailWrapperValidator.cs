using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class EmailWrapperValidator : AbstractValidator<EmailWrapper>
    {
        public EmailWrapperValidator()
        {
            RuleFor(x => x.Email)
                .NotNullOrEmptyString()
                .MustBeValidEmail();
        }
    }
}
