using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizationService.Repositories.Entities;
using AuthorizationService.Repositories.DAL;

namespace AuthorizationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAuthDBContextFactory _factory;

        public UserRepository(IAuthDBContextFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<User> GetUsers()
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.User;
            }
        }

        public User GetUser(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.User.FirstOrDefault(t => t.Id == id);
            }
        }

        public User GetUserByName(string name)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.User.FirstOrDefault(t => t.UserName == name);
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.User.FirstOrDefault(t => t.Email == email);
            }
        }

        public IEnumerable<Guid> GetUserRoles(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.UserRole.Where(t => t.UserId == id).Select(t => t.RoleId).ToList();
            }
        }

        public User CreateUser(string email, string userName, string password, string salt)
        {
            using (var context = _factory.CreateDBContext())
            {
                var users = context.Set<User>();

                var user = new User
                {
                    UserName = userName,
                    IsActive = true,
                    Email = email,
                    Password = password,
                    Salt = salt,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                users.Add(user);

                context.SaveChanges();

                return GetUser(user.Id);
            }
        }

        public User UpdateUser(Guid id, string email, string userName)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.User.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.UserName = userName;
                    result.Email = email;
                    result.UpdatedDate = DateTime.UtcNow;
                    context.SaveChanges();
                }
                else
                {
                    return null;
                }

                return GetUser(id);
            }
        }

        public User UpdateActive(Guid id, bool isActive)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.User.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.IsActive = isActive;
                    result.UpdatedDate = DateTime.UtcNow;
                    context.SaveChanges();
                }
                else
                {
                    return null;
                }

                return GetUser(id);
            }
        }

        public bool ChangePassword(Guid id, string password, string salt)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.User.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.Password = password;
                    result.Salt = salt;
                    result.UpdatedDate = DateTime.UtcNow;
                    context.SaveChanges();
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        public void UpdateRoles(Guid id, IEnumerable<Guid> roleIds)
        {
            using (var context = _factory.CreateDBContext())
            {
                var currentRoles = context.UserRole.Where(u => u.UserId == id).ToList();

                var rolesForDelete = currentRoles.Where(t => !roleIds.Contains(t.RoleId)).ToList();
                var rolesForAdded = roleIds.Except(currentRoles.Select(t => t.RoleId)).ToList();

                foreach (var role in rolesForDelete)
                {
                    context.UserRole.Remove(role);
                }

                foreach (var role in rolesForAdded)
                {
                    context.UserRole.Add(new UserRole { UserId = id, RoleId = role });
                }

                if (rolesForAdded.Any() || rolesForDelete.Any())
                {
                    context.SaveChanges();
                }
            }
        }
    }
}
