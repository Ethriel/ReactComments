using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReactComments.DAL;
using ReactComments.Server.Filters;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Implementation;
using ReactComments.Services.Model;
using ReactComments.Services.Utility;
using ReactComments.Services.Validation.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ReactComments.Server.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAppPackageServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mapperUtility = new MapperProfilesUtility();
            var isDocker = configuration.GetValue<bool>("IsDocker", false);
            var connectionString = isDocker ? configuration.GetConnectionString("Docker") : configuration.GetConnectionString("Default");

            return services.AddDbContext<CommentsDbContext>(options => options.UseSqlServer(connectionString))
                           .AddAutoMapper(cfg => { }, mapperUtility.GetAllMapperProfiles())
                           .AddFluentValidationAutoValidation(config =>
                           {
                               config.DisableBuiltInModelValidation = true;
                               config.ValidationStrategy = SharpGrip.FluentValidation.AutoValidation.Mvc.Enums.ValidationStrategy.Annotations;
                               config.EnableBodyBindingSourceAutomaticValidation = true;
                               config.EnableFormBindingSourceAutomaticValidation = true;
                               config.EnableQueryBindingSourceAutomaticValidation = true;
                           });
        }
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services.AddScoped<DbContext, CommentsDbContext>()
                           .AddScoped(typeof(IEntityService<>), typeof(EntityService<>))
                           .AddScoped(typeof(IEntityExtendedService<>), typeof(EntityExtendedService<>))
                           .AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>))
                           .AddScoped<IPersonService, PersonService>()
                           .AddScoped<IFileService, FileService>()
                           .AddScoped<ICommentService, CommentService>()
                           .AddScoped<IAuthService, AuthService>()
                           .AddScoped<ValidateRecaptchaAttribute>();
                           
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddScoped<IValidator<CommentDTO>, CommentDTOValidator>()
                           .AddScoped<IValidator<PersonDTO>, PersonDTOValidator>()
                           .AddScoped<IValidator<PersonAuth>, PersonAuthValidator>()
                           .AddScoped<IValidator<PersonSignUp>, PersonSignUpValidator>()
                           .AddScoped<IValidator<EmailWrapper>, EmailWrapperValidator>()
                           //.AddScoped<IValidator<CommentUploadFile>, CommentUploadFileValidator>()
                           .AddScoped<IValidator<SubmitComment>, SubmitCommentValidator>()
                           .AddScoped<CaptchaValidator>();
        }

        public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        {
            return services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    }

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });
        }
    }
}
