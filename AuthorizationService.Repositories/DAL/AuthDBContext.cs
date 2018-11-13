using Microsoft.EntityFrameworkCore;
using AuthorizationService.Repositories.Entities;

namespace AuthorizationService.Repositories.DAL
{
    public class AuthDBContext : DbContext, IAuthDBContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        {
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Session> UserSession { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.Name).HasColumnName("Name");
                b.Property(t => t.Description).HasColumnName("Description");
                b.ToTable("Role");
            });

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.IsActive).HasColumnName("IsActive");
                b.Property(t => t.Email).HasColumnName("Email");
                b.Property(t => t.UserName).HasColumnName("UserName");
                b.Property(t => t.Password).HasColumnName("Password");
                b.Property(t => t.Salt).HasColumnName("Salt");
                b.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
                b.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
                b.ToTable("User");
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(u => u.UserId);
                b.Property(t => t.UserId).HasColumnName("UserId");
                b.Property(t => t.RoleId).HasColumnName("RoleId");
                b.ToTable("UserRole");
            });

            builder.Entity<Session>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.UserId).HasColumnName("UserId");
                b.Property(t => t.Ticket).HasColumnName("Ticket");
                b.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
                b.Property(t => t.ExpiredDate).HasColumnName("ExpiredDate");
                b.Property(t => t.LastAccessDate).HasColumnName("LastAccessDate");
                b.Property(t => t.UpdateExpireInc).HasColumnName("UpdateExpire");
                b.Property(t => t.IP).HasColumnName("ip");
                b.ToTable("Session");
            });
        }
    }
}
