using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AuthorizationService.Business.Models;
using System.Text;
using AuthorizationService.Repositories;

namespace AuthorizationService.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;

        public UserService(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = _userRepository.GetUsers();

            return users.Select(t => new User(t));
        }

        public User GetUser(Guid id)
        {
            var user = _userRepository.GetUser(id);

            return user == null ? null : new User(user);
        }

        public User CheckUser(string userName, string password)
        {
            var user = GetUserByName(userName);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var hashedPassword = HashPassword(password, user.Salt);

            return (hashedPassword == user.Password) ? user : null;
        }

        public User GetUserByName(string name)
        {
            var user = _userRepository.GetUserByName(name);

            return user == null ? null : new User(user);
        }

        public User GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);

            return user == null ? null : new User(user);
        }

        public IEnumerable<int> GetUserRoles(Guid id)
        {
            return _userRepository.GetUserRoles(id);
        }
        
        public IEnumerable<Session> GetSessions(Guid userId, bool isActive = true)
        {
            var sessions = (isActive) 
                ? _sessionRepository.GetActiveSessions(userId) 
                : _sessionRepository.GetSessions(userId);

            return sessions.Select(t => new Session(t));
        }

        public User CreateUser(string email, string userName, string password)
        {
            var salt = GenerateSalt(10, false);

            var hashedPassword = HashPassword(password, salt);

            var createdUser = _userRepository.CreateUser(email, userName, hashedPassword, salt);

            return createdUser == null ? null : new User(createdUser);
        }

        public bool ChangePassword(Guid id, string newPassword)
        {
            var salt = GenerateSalt(10, false);

            var hashedPassword = HashPassword(newPassword, salt);

            var result = _userRepository.ChangePassword(id, hashedPassword, salt);

            if (!result)
            {
                return false;
            }

            _sessionRepository.CloseSessions(id);

            return true;
        }

        public User UpdateUser(Guid id, string email, string userName)
        {
            var updatedUser = _userRepository.UpdateUser(id, email, userName);

            return updatedUser == null ? null : new User(updatedUser);
        }

        public User UpdateActive(Guid id, bool isActive)
        {
            var updatedUser = _userRepository.UpdateActive(id, isActive);

            return updatedUser == null ? null : new User(updatedUser);
        }

        public void UpdateRoles(Guid id, IEnumerable<int> roleIds)
        {
            _userRepository.UpdateRoles(id, roleIds);
        }

        private string GenerateSalt(int len, bool onlyDigits)
        {
            var result = string.Empty;
            var alphanum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            var num = "0123456789".ToCharArray();

            for (var i = 0; i < len; ++i)
            {
                if (onlyDigits)
                {
                    result += num[new Random().Next(0, (num.Length - 1))];
                }
                else
                {
                    result += alphanum[(new Random().Next(0, (alphanum.Length - 1)))];
                }
            }

            return result;
        }

        private string HashPassword(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var plainTextWithSaltBytes =
                    new byte[passwordBytes.Length + saltBytes.Length];

            for (var i = 0; i < passwordBytes.Length; i++)
                plainTextWithSaltBytes[i] = passwordBytes[i];

            for (var i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[passwordBytes.Length + i] = saltBytes[i];

            var hashBytes = algorithm.ComputeHash(plainTextWithSaltBytes);

            var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (var i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (var i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            return Convert.ToBase64String(hashWithSaltBytes);
        }
    }
}