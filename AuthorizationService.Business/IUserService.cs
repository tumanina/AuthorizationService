using System;
using System.Collections.Generic;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Business
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUser(Guid id);
        User CheckUser(string userName, string password);
        User GetUserByName(string name);
        User GetUserByEmail(string email);
        IEnumerable<Guid> GetUserRoles(Guid id);
        IEnumerable<Session> GetSessions(Guid userId, bool isActive = true);
        User CreateUser(string email, string userName, string password);
        bool ChangePassword(Guid id, string newPassword);
        User UpdateUser(Guid id, string email, string userName);
        User UpdateActive(Guid id, bool isActive);
        void UpdateRoles(Guid id, IEnumerable<Guid> roleIds);
    }
}
