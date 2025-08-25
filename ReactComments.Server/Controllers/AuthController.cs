using Microsoft.AspNetCore.Mvc;
using ReactComments.Server.Extensions;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace ReactComments.Server.Controllers
{
    [AutoValidation]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] PersonAuth personAuth)
        {
            var result = await authService.SignInAsync(personAuth);

            return this.ActionResultByApiResult(result, logger);
        }
        
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] PersonAuth personAuth)
        {
            var result = await authService.SignUpAsync(personAuth);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut([FromBody] EmailWrapper emailWrapper)
        {
            var result = await authService.SignOutAsync(emailWrapper.Email);

            return this.ActionResultByApiResult(result, logger);
        }
    }
}
