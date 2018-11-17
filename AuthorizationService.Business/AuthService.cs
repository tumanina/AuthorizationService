using AuthorizationService.Business.Models;
using AuthorizationService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationService.Business
{
    public class AuthService : IAuthService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserService _userService;

        public AuthService(ISessionRepository sessionRepository, IUserService userService)
        {
            _sessionRepository = sessionRepository;
            _userService = userService;
        }

        public LoginResult Login(string userName, string password, string ip)
        {
            var user = _userService.CheckUser(userName, password);

            if (user == null)
            {
                return new LoginResult { IsAuth = false };
            }

            var session = _sessionRepository.CreateSession(user.Id, 900, ip);

            return new LoginResult { IsAuth = true, Ticket  = session.Ticket };
        }

        public AuthResult CheckToken(string token, IEnumerable<Role> roles)
        {
            var session = _sessionRepository.GetSessionByTicket(token);

            if (session == null)
            {
                return new AuthResult { StatusCode = 401 };
            }

            if (session.ExpiredDate < DateTime.UtcNow)
            {
                return new AuthResult { StatusCode = 401, Message = "Session expired." };
            }

            var user = _userService.GetUser(session.UserId);

            if (user == null || !user.IsActive)
            {
                return new AuthResult { StatusCode = 401, Message = "User not found or blocked." };
            }

            if (roles.Any())
            {
                var userRoles = _userService.GetUserRoles(session.UserId);

                return userRoles.Any(roles.Select(t => (int)t).Contains)
                    ? new AuthResult { StatusCode = 200, SessionId = session.Id, UserId = user.Id }
                    : new AuthResult { StatusCode = 403, Message = "Forbidden" };
            }
            else
            {
                return new AuthResult {StatusCode = 200, SessionId = session.Id, UserId = user.Id};
            }
        }

        public AuthResult Registrate(string token, IEnumerable<Role> roles)
        {
            var checkTokenResult = CheckToken(token, roles);

            if (checkTokenResult.StatusCode == 200 && checkTokenResult.SessionId.HasValue)
            {
                _sessionRepository.ProlongSession(checkTokenResult.SessionId.Value);
            }

            return checkTokenResult;
        }
    }
}
