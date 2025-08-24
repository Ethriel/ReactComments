using FluentValidation;

namespace ReactComments.Services.Validation.Extensions
{
    public static class AuthenticationValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .EmailAddress().WithMessage("Incorrect email format!");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[!@#$%^&*(),.?""{}|<>_\-+=;:'\\[\]/~`]").WithMessage("Password must contain at least one special character.");
        }
    }
}
