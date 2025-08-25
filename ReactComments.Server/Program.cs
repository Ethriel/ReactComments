using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReactComments.DAL;
using ReactComments.DAL.Model;
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

            builder.Services.AddIdentity<Person, AppRole>()
                            .AddEntityFrameworkStores<CommentsDbContext>()
                            .AddRoles<AppRole>()
                            .AddDefaultTokenProviders();

            builder.Services.AddAppPackageServices(builder.Configuration)
                            .AddAppServices()
                            .AddValidators()
                            .ConfigureCookies();

            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CommentsDbContext>();
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                    dbContext.Database.Migrate();

                if (!dbContext.Roles.Any())
                {
                    dbContext.Roles.Add(new AppRole { Name = "USER", NormalizedName = "User" });
                    dbContext.Roles.Add(new AppRole { Name = "ADMIN", NormalizedName = "Admin" });
                    dbContext.SaveChanges();
                }
            }

            app.UseDefaultFiles();
            app.MapStaticAssets();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var result = JsonConvert.SerializeObject(new { error = exception?.Message });
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }));

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
