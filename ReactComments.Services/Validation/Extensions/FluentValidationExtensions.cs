using FluentValidation;

namespace ReactComments.Services.Validation.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> NotNullOrEmptyString<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage("{PropertyName} can't be null!")
                .NotEmpty().WithMessage("{PropertyName} can't be empty!");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(value => Guid.TryParse(value, out _)).WithMessage("{PropertyName} has invalid format!")
                .Must(value => Guid.TryParse(value, out var guid) && guid != Guid.Empty).WithMessage("{PropertyName} can't be a default Guid!");
        }

        public static IRuleBuilderOptions<T, byte[]?> MustBeValidImageByteArray<T>(this IRuleBuilder<T, byte[]?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("{PropertyName} can't be empty!");
        }

        public static IRuleBuilderOptions<T, int> MustBeValidDigit<T>(this IRuleBuilder<T, int> rulebuilder)
        {
            return rulebuilder
                .Must(value => value > 0).WithMessage("{PropertyName} must be greater than zero!");
        }
    }
}
