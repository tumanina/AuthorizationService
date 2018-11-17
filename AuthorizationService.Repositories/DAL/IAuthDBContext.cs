using Microsoft.EntityFrameworkCore;
using AuthorizationService.Repositories.Entities;
using System;

namespace AuthorizationService.Repositories.DAL
{
    public interface IAuthDBContext: IDisposable
    {
        DbSet<User> User { get; set; }
        DbSet<UserRole> UserRole { get; set; }
        DbSet<Session> UserSession { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
