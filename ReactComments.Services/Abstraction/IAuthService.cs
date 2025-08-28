using ReactComments.Services.Model;
using ReactComments.Services.Utility.ApiResult.Abstraction;

namespace ReactComments.Services.Abstraction
{
    public interface IAuthService
    {
        Task AddRolesAsync();
        IApiResult SignIn(PersonAuth personAuth);
        Task<IApiResult> SignInAsync(PersonAuth personAuth);
        IApiResult SignUp(PersonSignUp personAuth);
        Task<IApiResult> SignUpAsync(PersonSignUp personSignUp);
        IApiResult SignOut(string email);
        Task<IApiResult> SignOutAsync(string email);
    }
}
