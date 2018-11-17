using System;
using System.Collections.Generic;
using AuthorizationService.Repositories.Entities;

namespace AuthorizationService.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(Guid id);
        User GetUserByName(string name);
        User GetUserByEmail(string email);
        IEnumerable<int> GetUserRoles(Guid id);
        User CreateUser(string email, string userName, string password, string salt);
        User UpdateUser(Guid id, string email, string userName);
        User UpdateActive(Guid id, bool isActive);
        bool ChangePassword(Guid id, string password, string salt);
        void UpdateRoles(Guid id, IEnumerable<int> roleIds);
    }
}
