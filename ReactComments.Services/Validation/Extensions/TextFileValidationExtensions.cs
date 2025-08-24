using FluentValidation;

namespace ReactComments.Services.Validation.Extensions
{
    public static class TextFileValidationExtensions
    {
        private static readonly string[] allowedTextFileTypes =
        {
            ".txt"
        };
        public static IRuleBuilderOptions<T, string?> MustBeValidTextFileType<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{PropertyName} can't be empty!")
                .Must(fileName =>
                {
                    var ext = Path.GetExtension(fileName?.Trim());
                    return allowedTextFileTypes.Contains(ext);
                }).WithMessage($"Unsupported file extension. Allowed extensions: {string.Join(", ", allowedTextFileTypes)}");
        }
    }
}
