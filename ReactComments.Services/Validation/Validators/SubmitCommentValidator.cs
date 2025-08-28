using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class SubmitCommentValidator : AbstractValidator<SubmitComment>
    {
        public SubmitCommentValidator()
        {
            RuleFor(c => c.Text)
                .NotNullOrEmptyString()
                .MustBeAllowedHtml();

            RuleFor(c => c.File)
                .Must(f => f?.Length > 0).WithMessage("File can't be empty!")
                .When(c => c.File is not null);

            RuleFor(c => c.PersonId)
                .MustBeValidDigit();

            //RuleFor(c => c.PersonName)
            //    .NotNullOrEmptyString();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            //When(c => c.ParentCommentId != null, () =>
            //{
            //    RuleFor(c => c.ParentCommentId)
            //    .Cascade(CascadeMode.Stop)
            //    .MustBeValidGuid();
            //});
            RuleFor(c => c.ParentCommentId)
                .Cascade(CascadeMode.Stop)
                .MustBeValidGuid()
                .When(c => c.ParentCommentId != null, ApplyConditionTo.CurrentValidator);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }
    }
}
