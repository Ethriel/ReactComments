using FluentValidation;

namespace ReactComments.Services.Validation.Extensions
{
    public static class ImageValidationExtensions
    {
        private static readonly string[] allowedMimeTypes =
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };
        public static IRuleBuilderOptions<T, string?> MustBeValidImageType<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{PropertyName} can't be empty!")
                .Must(fileName =>
                {
                    var ext = Path.GetExtension(fileName?.Trim());
                    return allowedMimeTypes.Contains(ext);
                }).WithMessage("Unsupported image type.");
        }
    }
}
