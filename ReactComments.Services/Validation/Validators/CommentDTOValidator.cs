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

            RuleFor(c => c.ImageAttachment.Contents)
                .MustBeValidImageByteArray()
                .When(c => c.ImageAttachment is not null);

            RuleFor(c => c.ImageAttachment.Name)
                .MustBeValidImageType()
                .When(c => c.ImageAttachment is not null);

            RuleFor(c => c.TextFileAttachment.Contents)
                .MustBeValidImageByteArray()
                .When(c => c.TextFileAttachment is not null);

            RuleFor(c => c.TextFileAttachment.Name)
                .MustBeValidTextFileType()
                .When(c => c.TextFileAttachment is not null);
        }
    }
}
