using AuthorizationService.Business.Models;
using System.Collections.Generic;

namespace AuthorizationService.Business
{
    public interface IAuthService
    {
        LoginResult Login(string userName, string password, string ip);
        AuthResult CheckToken(string token, IEnumerable<Role> roles);
        AuthResult Registrate(string token, IEnumerable<Role> roles);
    }
}
