
using Microsoft.EntityFrameworkCore;
using ReactComments.DAL;
using ReactComments.Server.Extensions;
using Scalar.AspNetCore;

namespace ReactComments.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole();
            builder.Services.AddSingleton<IConfiguration>(provider => builder.Configuration);

            builder.Services.AddControllers();

            var isDocker = builder.Configuration.GetValue<bool>("IsDocker");
            var connectionString = isDocker ? builder.Configuration.GetConnectionString("Docker") : builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<CommentsDbContext>(options => options.UseSqlServer(connectionString))
                            .AddAppServices();

            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            app.UseDefaultFiles();
            app.MapStaticAssets();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
