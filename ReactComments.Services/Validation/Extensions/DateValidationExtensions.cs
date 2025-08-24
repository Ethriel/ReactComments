using FluentValidation;

namespace ReactComments.Services.Validation.Extensions
{
    public static class DateValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidIsoDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(dateString => DateTime.TryParse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date))
                .WithMessage("{PropertyName} has invalid format!");
        }
        public static IRuleBuilderOptions<T, string> MustBeValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(dateString =>
                {
                    if (!DateTime.TryParse(dateString, out var date)) return false;
                    return date != DateTime.MinValue;
                })
                .WithMessage("{PropertyName} must be a valid non-default date!");
        }
    }
}
