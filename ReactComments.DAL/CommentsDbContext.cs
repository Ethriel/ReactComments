using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReactComments.DAL.Model;

namespace ReactComments.DAL
{
    public class CommentsDbContext : IdentityDbContext<Person, AppRole, int>
    {
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual IConfiguration Configuration { get; set; }

        public CommentsDbContext(IConfiguration configuration) : base() { Configuration = configuration; }

        public CommentsDbContext(DbContextOptions options, IConfiguration configuration) : base(options) { Configuration = configuration; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var isDocker = Configuration.GetValue("IsDocker", false);
                var connectionString = isDocker ? Configuration.GetConnectionString("Docker") : Configuration.GetConnectionString("Default");

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString(connectionString));
            }
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Person)
                      .HasForeignKey(c => c.PersonId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AppRole>()
                .HasMany(ar => ar.People)
                .WithOne(p => p.AppRole)
                .HasForeignKey(p => p.RoleId);

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();

                entity.HasOne(c => c.ParentComment)
                      .WithMany(c => c.Replies)
                      .HasForeignKey(c => c.ParentCommentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.Text)
                      .HasMaxLength(100_000);
            });
        }
    }
}
