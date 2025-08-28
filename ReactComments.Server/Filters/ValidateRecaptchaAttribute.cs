using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReactComments.Services.Validation.Validators;

namespace ReactComments.Server.Filters
{
    public class ValidateRecaptchaAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public ValidateRecaptchaAttribute(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                var captchaValidator = context.HttpContext.RequestServices.GetRequiredService<CaptchaValidator>();

                var captchaToken = context.HttpContext.Request.Headers["X-Captcha-Token"].FirstOrDefault()
                                   ?? context.HttpContext.Request.Query["captcha"].FirstOrDefault();

                if (string.IsNullOrEmpty(captchaToken) || !await captchaValidator.ValidateAsync(captchaToken))
                {
                    context.Result = new BadRequestObjectResult(new { message = "Invalid CAPTCHA" });
                    return;
                }
            }

            await next();
        }
    }
}
