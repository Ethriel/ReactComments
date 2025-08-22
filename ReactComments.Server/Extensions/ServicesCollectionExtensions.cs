using Microsoft.EntityFrameworkCore;
using ReactComments.DAL;

namespace ReactComments.Server.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services.AddScoped<DbContext, CommentsDbContext>();
                           //.AddScoped(typeof(IEntityService<>), typeof(EntityService<>))
                           //.AddScoped(typeof(IEntityExtendedService<>), typeof(EntityExtendedService<>))
                           //.AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>))
                           //.AddScoped(typeof(IUtilityService), typeof(UtilityService));
        }
    }
}
