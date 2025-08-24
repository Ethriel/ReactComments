using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReactComments.DAL.Model;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;
using ReactComments.Services.Utility.ApiResult.Implementation;

namespace ReactComments.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<Person> signInManager;
        private readonly UserManager<Person> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly ILogger<IAuthService> logger;
        private readonly IMapperService<Person, PersonDTO> mapperService;

        public AuthService(SignInManager<Person> signInManager, UserManager<Person> userManager,
                           RoleManager<AppRole> roleManager, ILogger<IAuthService> logger,
                           IMapperService<Person, PersonDTO> mapperService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.mapperService = mapperService;
        }

        public async Task AddRolesAsync()
        {
            var user = new AppRole { Name = "USER", NormalizedName = "User" };
            var admin = new AppRole { Name = "ADMIN", NormalizedName = "Admin" };

            await roleManager.CreateAsync(user);
            await roleManager.CreateAsync(admin);
        }

        public IApiResult SignIn(PersonAuth personAuth)
        {
            return SignInAsync(personAuth).Result;
        }

        public async Task<IApiResult> SignInAsync(PersonAuth personAuth)
        {
            var apiResult = default(IApiResult);

            var user = await userManager.FindByEmailAsync(personAuth.Email);
            if (user is null)
            {
                var message = "Sign in has failed";
                var loggerMessage = $"Sign in has failed for {personAuth.Email}";
                var errors = new string[] { $"Email {personAuth.Email} is incorrect" };
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                var signInResult = await signInManager.PasswordSignInAsync(user, personAuth.Password, true, false);
                if (signInResult.Succeeded)
                {
                    var userDto = mapperService.MapDto(user);
                    apiResult = new ApiOkResult(data: userDto);
                }
                else
                {
                    var message = "Sign in has failed";
                    var errors = new string[] { "Password is incorrect" };
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, $"User email {user.Email} has failed to sign in with password", message, errors);
                }
            }

            return apiResult;
        }

        public IApiResult SignOut(string email)
        {
            return SignOutAsync(email).Result;
        }

        public async Task<IApiResult> SignOutAsync(string email)
        {
            var apiResult = default(IApiResult);

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var errorMessage = "User not found";
                var loggerMessage = $"User {email} not found";
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, loggerMessage, errorMessage, [errorMessage]);
            }
            else
            {
                var stampResult = await userManager.UpdateSecurityStampAsync(user);
                if (stampResult.Succeeded)
                {
                    await signInManager.SignOutAsync();
                    apiResult = new ApiOkResult(ApiResultStatus.Ok, message: "Signed out successfully");
                }
                else
                {
                    var errorMessage = "Sign out error";
                    var loggerMessage = $"{errorMessage} for user {email}";
                    var errors = GetIdentityErrors(stampResult.Errors);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, loggerMessage, errorMessage, errors);
                }

            }
            return apiResult;
        }

        public IApiResult SignUp(PersonAuth personAuth)
        {
            return SignUpAsync(personAuth).Result;
        }

        public async Task<IApiResult> SignUpAsync(PersonAuth personAuth)
        {
            var apiResult = default(IApiResult);
            var existingUser = await userManager.FindByEmailAsync(personAuth.Email);

            if (existingUser is not null)
            {
                var errorMessage = "Sign up has failed";
                var loggerMessage = $"New user {personAuth.Email} has tried to sign up. User with this email already exists";
                var errors = new string[] { $"User {personAuth.Email} is already registered. Try to sign in" };
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, loggerMessage, errorMessage, errors);
            }
            else
            {
                var role = await roleManager.FindByNameAsync("USER");
                var user = new Person { Email = personAuth.Email, AppRole = role };

                var creationResult = await userManager.CreateAsync(user, personAuth.Password);
                if (creationResult.Succeeded)
                {
                    user = await userManager.FindByEmailAsync(user.Email);
                    var userDto = mapperService.MapDto(user);
                    apiResult = new ApiOkResult(ApiResultStatus.Ok, data: userDto);
                }
                else
                {
                    var message = "Sign up failed";
                    var errors = GetIdentityErrors(creationResult.Errors);
                    var loggerMessage = $"New user {user.Email} has failed to sign up. Errors: {errors}";
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
                }
            }

            return apiResult;
        }

        private IEnumerable<string> GetIdentityErrors(IEnumerable<IdentityError> errorsCollection)
        {
            return errorsCollection.Select(x => x.Description);
        }
    }
}
