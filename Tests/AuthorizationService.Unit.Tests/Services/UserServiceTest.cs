using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AuthorizationService.Business;
using AuthorizationService.Repositories;
using AuthorizationService.Repositories.Entities;

namespace AuthorizationService.Unit.Tests.ServiceTests
{
    [TestClass]
    public class UserServiceTest
    {
        private static readonly Mock<IUserRepository> UserRepository = new Mock<IUserRepository>();
        private static readonly Mock<ISessionRepository> SessionRepository = new Mock<ISessionRepository>();

        [TestMethod]
        public void GetUsers_UsersExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var email1 = "email1@mail.ru";
            var email2 = "email2@mail.ru";
            var email3 = "email3@mail.ru";
            var username1 = "name1";
            var username2 = "name2";
            var username3 = "name3";
            var salt1 = "54ythgdh9";
            var salt2 = "22ythgdh9";
            var salt3 = "33ythgdh9";
            var secret1 = "542gythnfsli8";
            var secret2 = "111gythnfsli8";
            var secret3 = "333gythnfsli8";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var updateDate1 = DateTime.UtcNow.AddMinutes(-3);
            var updateDate2 = DateTime.UtcNow.AddMinutes(-6);
            var updateDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<User>
            {
                new User { Id = id1, UserName = username1, Email = email1, Salt = salt1, Password = secret1, CreatedDate = createDate1, UpdatedDate = updateDate1, IsActive = true },
                new User { Id = id2, UserName = username2, Email = email2, Salt = salt2, Password = secret2, CreatedDate = createDate2, UpdatedDate = updateDate2, IsActive = true },
                new User { Id = id3, UserName = username3, Email = email3, Salt = salt3, Password = secret3, CreatedDate = createDate3, UpdatedDate = updateDate3, IsActive = true }
            };

            UserRepository.Setup(x => x.GetUsers()).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUsers();

            UserRepository.Verify(x => x.GetUsers(), Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserName == username1 && t.Email == email1 && t.Salt == salt1 && t.CreatedDate == createDate1 && t.Password == secret1 && t.UpdatedDate == updateDate1 && t.IsActive == true));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserName == username2 && t.Email == email2 && t.Salt == salt2 && t.CreatedDate == createDate2 && t.Password == secret2 && t.UpdatedDate == updateDate2 && t.IsActive == true));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserName == username3 && t.Email == email3 && t.Salt == salt3 && t.CreatedDate == createDate3 && t.Password == secret3 && t.UpdatedDate == updateDate3 && t.IsActive == true));
        }

        [TestMethod]
        public void GetUsers_UsersNotExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var data = new List<User>();

            UserRepository.Setup(x => x.GetUsers()).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUsers();

            UserRepository.Verify(x => x.GetUsers(), Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetUser_UserExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name2";
            var salt = "22ythgdh9";
            var secret = "111gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-2);
            var updateDate = DateTime.UtcNow.AddMinutes(-6);

            var entity = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.GetUser(id)).Returns(entity);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUser(id);

            UserRepository.Verify(x => x.GetUser(id), Times.Once);
            Assert.AreEqual(result.UserName, username);
            Assert.AreEqual(result.Email, email);
            Assert.AreEqual(result.Salt, salt);
            Assert.AreEqual(result.Password, secret);
            Assert.AreEqual(result.CreatedDate, createDate);
            Assert.AreEqual(result.UpdatedDate, updateDate);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetUser_UserNotExisted_ShouldReturnNull()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();

            UserRepository.Setup(x => x.GetUser(id)).Returns((User)null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUser(id);

            UserRepository.Verify(x => x.GetUser(id), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUsersByName_UsersExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.GetUserByName(username)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserByName(username);

            UserRepository.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result.UserName, username);
            Assert.AreEqual(result.Email, email);
            Assert.AreEqual(result.Salt, salt);
            Assert.AreEqual(result.Password, secret);
            Assert.AreEqual(result.CreatedDate, createDate);
            Assert.AreEqual(result.UpdatedDate, updateDate);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetUsersByName_UsersNotExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();
            var username = "name1";
 
            UserRepository.Setup(x => x.GetUserByName(username)).Returns((User) null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserByName(username);

            UserRepository.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUserByEmail_UsersExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.GetUserByEmail(email)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserByEmail(email);

            UserRepository.Verify(x => x.GetUserByEmail(email), Times.Once);
            Assert.AreEqual(result.UserName, username);
            Assert.AreEqual(result.Email, email);
            Assert.AreEqual(result.Salt, salt);
            Assert.AreEqual(result.Password, secret);
            Assert.AreEqual(result.CreatedDate, createDate);
            Assert.AreEqual(result.UpdatedDate, updateDate);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetUserByEmail_UsersNotExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var email = "email2@mail.ru";

            UserRepository.Setup(x => x.GetUserByEmail(email)).Returns((User)null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserByEmail(email);

            UserRepository.Verify(x => x.GetUserByEmail(email), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CheckUser_UsersExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = HashPassword(password, salt);
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.GetUserByName(username)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.CheckUser(username, password);

            UserRepository.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result.UserName, username);
            Assert.AreEqual(result.Email, email);
            Assert.AreEqual(result.Salt, salt);
            Assert.AreEqual(result.Password, secret);
            Assert.AreEqual(result.CreatedDate, createDate);
            Assert.AreEqual(result.UpdatedDate, updateDate);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void CheckUser_UsersNotExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var username = "name1";
            var password = "123654";

            UserRepository.Setup(x => x.GetUserByName(username)).Returns((User)null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.CheckUser(username, password);

            UserRepository.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CheckUser_InvalidPassword_ShouldReturnNull()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = HashPassword(password, salt);
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.GetUserByName(username)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.CheckUser(username, "123123");

            UserRepository.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUserRoles_UserRolesExisted_ShouldReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var roleId2 = 2;
            var roleId3 = 3;
            var roleId4 = 4;
            var roleId5 = 5;

            var roles = new List<UserRole>
            {
                new UserRole { UserId = userId, RoleId = roleId2 },
                new UserRole { UserId = userId, RoleId = roleId3 },
                new UserRole { UserId = userId, RoleId = roleId4 },
                new UserRole { UserId = userId, RoleId = roleId5 }
            };

            UserRepository.Setup(x => x.GetUserRoles(userId)).Returns(roles.Select(t => t.RoleId));

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserRoles(userId);

            Assert.AreEqual(result.Count(), 4);
            Assert.IsTrue(result.Contains(roleId2));
            Assert.IsTrue(result.Contains(roleId3));
            Assert.IsTrue(result.Contains(roleId4));
            Assert.IsTrue(result.Contains(roleId5));
        }

        [TestMethod]
        public void GetUserRoles_UserRolesNotExisted_ShouldReturnEmptyList()
        {
            UserRepository.ResetCalls();

            var userId = Guid.NewGuid();

            UserRepository.Setup(x => x.GetUserRoles(userId)).Returns(new List<int>());

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetUserRoles(userId);

            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetAllUserSessions_SessionsExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId= Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket2 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval2 = 1900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip2 = "127.0.0.2";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate2 = DateTime.UtcNow.AddDays(2);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            SessionRepository.Setup(x => x.GetSessions(userId)).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetSessions(userId, false);

            SessionRepository.Verify(x => x.GetSessions(userId), Times.Once);
            SessionRepository.Verify(x => x.GetActiveSessions(userId), Times.Never);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetAllUserSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var userId= Guid.NewGuid();

            var data = new List<Session>();

            SessionRepository.Setup(x => x.GetSessions(userId)).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetSessions(userId, false);

            SessionRepository.Verify(x => x.GetSessions(userId), Times.Once);
            SessionRepository.Verify(x => x.GetActiveSessions(userId), Times.Never);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetActiveUserSessions_SessionsExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId= Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket2 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval2 = 1900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip2 = "127.0.0.2";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate2 = DateTime.UtcNow.AddDays(2);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Repositories.Entities.Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            SessionRepository.Setup(x => x.GetActiveSessions(userId)).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetSessions(userId);

            SessionRepository.Verify(x => x.GetSessions(userId), Times.Never);
            SessionRepository.Verify(x => x.GetActiveSessions(userId), Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetActiveUserSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var userId= Guid.NewGuid();

            var data = new List<Session>();

            SessionRepository.Setup(x => x.GetActiveSessions(userId)).Returns(data);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.GetSessions(userId);

            SessionRepository.Verify(x => x.GetSessions(userId), Times.Never);
            SessionRepository.Verify(x => x.GetActiveSessions(userId), Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void CreateUser_Success_ShouldReturnUser()
        {
            UserRepository.ResetCalls();
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var generatedSalt = string.Empty;
            var generatedPassword = string.Empty;

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.CreateUser(email, username, It.IsAny<string>(), It.IsAny<string>())).Returns(user)
                .Callback((string emailParam, string nameParam, string secretParam, string saltParam) => { generatedSalt = saltParam; generatedPassword = secretParam; });

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.CreateUser(email, username, password);

            UserRepository.Verify(x => x.CreateUser(email, username, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.Email, email);
            Assert.AreEqual(result.UserName, username);
            Assert.AreEqual(generatedPassword, HashPassword(password, generatedSalt));
        }

        [TestMethod]
        public void CreateUser_ServiceReturnNull_ShouldReturnNull()
        {
            UserRepository.ResetCalls();

            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";

            UserRepository.Setup(x => x.CreateUser(email, username, It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.CreateUser(email, username, password);

            UserRepository.Verify(x => x.CreateUser(email, username, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void ChangePassword_UserExisted_ShouldGenerateCorrectHashCloseSession()
        {
            UserRepository.ResetCalls();
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var sessionId1 = Guid.NewGuid();
            var sessionId2 = Guid.NewGuid();
            var sessionId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket2 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval2 = 1900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip2 = "127.0.0.2";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var sessions = new List<Session>
            {
                new Session { Id = sessionId1, UserId = id, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = DateTime.UtcNow.AddSeconds(-1) },
                new Session { Id = sessionId2, UserId = id, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = DateTime.UtcNow.AddSeconds(-1) },
                new Session { Id = sessionId3, UserId = id, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = DateTime.UtcNow.AddSeconds(-1) },
            };

            var newPassword = "123654";

            var generatedSalt = string.Empty;
            var generatedPassword = string.Empty;
            
            UserRepository.Setup(x => x.ChangePassword(id, It.IsAny<string>(), It.IsAny<string>())).Returns(true)
                .Callback((Guid idParam, string secretParam, string saltParam) => { generatedSalt = saltParam; generatedPassword = secretParam; });

            SessionRepository.Setup(x => x.CloseSessions(id)).Returns(sessions);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.ChangePassword(id, newPassword);

            UserRepository.Verify(x => x.ChangePassword(id, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            SessionRepository.Verify(x => x.CloseSessions(id), Times.Once);
            Assert.AreEqual(result, true);
            Assert.AreEqual(generatedPassword, HashPassword(newPassword, generatedSalt));
        }

        [TestMethod]
        public void ChangePassword_UserNotExisted_ShouldGenerateCorrectHashNotCloseSession()
        {
            UserRepository.ResetCalls();
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var sessionId1 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var ip1 = "127.0.0.1";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);

            var sessions = new List<Session>
            {
                new Session { Id = sessionId1, UserId = id, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = DateTime.UtcNow.AddSeconds(-1) },
            };

            var newPassword = "123654";

            var generatedSalt = string.Empty;
            var generatedPassword = string.Empty;

            UserRepository.Setup(x => x.ChangePassword(id, It.IsAny<string>(), It.IsAny<string>())).Returns(false)
                .Callback((Guid idParam, string secretParam, string saltParam) => { generatedSalt = saltParam; generatedPassword = secretParam; });

            SessionRepository.Setup(x => x.CloseSessions(id)).Returns(sessions);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.ChangePassword(id, newPassword);

            UserRepository.Verify(x => x.ChangePassword(id, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            SessionRepository.Verify(x => x.CloseSessions(id), Times.Never);
            Assert.AreEqual(result, false);
            Assert.AreEqual(generatedPassword, HashPassword(newPassword, generatedSalt));
        }

        [TestMethod]
        public void UpdateUser_Success_ShouldReturnUser()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var newEmail = "email22@mail.ru";
            var newUsername = "name22";

            var user = new User { Id = id, UserName = newUsername, Email = newEmail, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.UpdateUser(id, newEmail, newUsername)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.UpdateUser(id, newEmail, newUsername);

            UserRepository.Verify(x => x.UpdateUser(id, newEmail, newUsername), Times.Once);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void UpdateUser_UserNotExisted_ShouldReturnNull()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();

            var newEmail = "email22@mail.ru";
            var newUsername = "name22";

            UserRepository.Setup(x => x.UpdateUser(id, newEmail, newUsername)).Returns((User) null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.UpdateUser(id, newEmail, newUsername);

            UserRepository.Verify(x => x.UpdateUser(id, newEmail, newUsername), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void UpdateActive_Success_ShouldReturnUser()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var isActive = true;

            var newEmail = "email22@mail.ru";
            var newUsername = "name22";

            var user = new User { Id = id, UserName = newUsername, Email = newEmail, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserRepository.Setup(x => x.UpdateActive(id, isActive)).Returns(user);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.UpdateActive(id, isActive);

            UserRepository.Verify(x => x.UpdateActive(id, isActive), Times.Once);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void UpdateActive_ServiceReturnNull_ShouldReturnNull()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var isActive = true;

            UserRepository.Setup(x => x.UpdateActive(id, isActive)).Returns((User) null);

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            var result = service.UpdateActive(id, isActive);

            UserRepository.Verify(x => x.UpdateActive(id, isActive), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void UpdateUserRoles_UserRolesExisted_UseDbContextReturnCorrect()
        {
            UserRepository.ResetCalls();

            var id = Guid.NewGuid();
            var roleId2 = 2;
            var roleId3 = 3;
            var roleId4 = 4;

            UserRepository.Setup(x => x.UpdateRoles(id, new List<int> { roleId2, roleId3, roleId4 }));

            var service = new UserService(UserRepository.Object, SessionRepository.Object);

            service.UpdateRoles(id, new List<int> { roleId2, roleId3, roleId4 });

            UserRepository.Verify(x => x.UpdateRoles(id, new List<int> { roleId2, roleId3, roleId4 }), Times.Once);
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
