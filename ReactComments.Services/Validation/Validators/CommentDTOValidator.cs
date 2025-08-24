using FluentValidation;
using ReactComments.Services.Model;
using ReactComments.Services.Validation.Extensions;

namespace ReactComments.Services.Validation.Validators
{
    public class CommentDTOValidator : AbstractValidator<CommentDTO>
    {
        public CommentDTOValidator()
        {
            RuleFor(c => c.Id)
                .NotNullOrEmptyString()
                .MustBeValidGuid();

            RuleFor(c => c.Text)
                .NotNullOrEmptyString()
                .MustBeAllowedHtml();

            RuleFor(c => c.CreatedAt)
                .NotNullOrEmptyString()
                .MustBeValidIsoDate();

            RuleFor(c => c.UpdatedAt)
                .NotNullOrEmptyString()
                .MustBeValidIsoDate();

            RuleFor(c => c.Image)
                .MustBeValidImageByteArray()
                .When(c => c.Image is not null);

            RuleFor(c => c.ImageMimeType)
                .MustBeValidImageType()
                .When(c => c.Image is not null);

            RuleFor(c => c.TextFile)
                .MustBeValidImageByteArray()
                .When(c => c.TextFile is not null);

            RuleFor(c => c.TextFileName)
                .MustBeValidTextFileType()
                .When(c => c.TextFile is not null && c.TextFileName is not null);
        }
    }
}
