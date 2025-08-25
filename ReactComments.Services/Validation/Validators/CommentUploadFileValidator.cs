using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class CommentUploadFileValidator : AbstractValidator<CommentUploadFile>
    {
        public CommentUploadFileValidator()
        {
            RuleFor(x => x.Comment)
                .SetValidator(new CommentDTOValidator());

            RuleFor(x => x.UserId)
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
