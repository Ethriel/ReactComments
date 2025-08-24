using Microsoft.EntityFrameworkCore;
using ReactComments.DAL;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Implementation;
using ReactComments.Services.Utility;

namespace ReactComments.Server.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAppPackageServices(this IServiceCollection services, IConfiguration configuration)
        {
            var isDocker = configuration.GetValue<bool>("IsDocker", false);
            var connectionString = isDocker ? configuration.GetConnectionString("Docker") : configuration.GetConnectionString("Default");

            return services.AddDbContext<CommentsDbContext>(options => options.UseSqlServer(connectionString))
                           .AddAutoMapper(cfg => { }, MapperProfilesUtility.MapperProfiles);
        }
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services.AddScoped<DbContext, CommentsDbContext>()
                           .AddScoped(typeof(IEntityService<>), typeof(EntityService<>))
                           .AddScoped(typeof(IEntityExtendedService<>), typeof(EntityExtendedService<>))
                           .AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>))
                           .AddScoped<IPersonService, PersonService>()
                           .AddScoped<ICommentService, CommentService>()
                           .AddScoped<IAuthService, AuthService>();
        }
    }
}
