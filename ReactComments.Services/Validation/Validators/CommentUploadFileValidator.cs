using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class CommentUploadFileValidator : AbstractValidator<CommentUploadFile>
    {
        public CommentUploadFileValidator()
        {
            RuleFor(x => x.CommentId)
                .NotNullOrEmptyString()
                .MustBeValidGuid();

            RuleFor(x => x.PersonId)
                .MustBeValidDigit();

            RuleFor(x => x.File)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.File.FileName)
                .NotNullOrEmptyString();

            RuleFor(x => x.File.ContentType)
                .NotNullOrEmptyString();
        }
    }
}
