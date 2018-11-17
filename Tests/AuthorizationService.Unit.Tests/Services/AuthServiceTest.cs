using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Unit.Tests.ServiceTests
{
    [TestClass]
    public class AuthServiceTest
    {
        private static readonly Mock<ISessionService> SessionService = new Mock<ISessionService>();
        private static readonly Mock<IUserService> UserService = new Mock<IUserService>();

        [TestMethod]
        public void Login_UserNameAndPasswordExist_ReturnSuccess()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var userName = "userName1";
            var password = "password1";
            var ip = "127.0.0.1";
            var id = Guid.NewGuid();
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var sessionId = Guid.NewGuid();
            var ticket = Guid.NewGuid().ToString();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow;
            var ExpiredDate = DateTime.UtcNow.AddMinutes(interval);
            var lastAccessDate = DateTime.UtcNow;

            UserService.Setup(x => x.CheckUser(userName, password)).Returns(new Business.Models.User
            {
                Id = id,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });

            SessionService.Setup(x => x.CreateSession(id, It.IsAny<int>(), ip)).Returns(new Session { Id = sessionId, UserId = id, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.Login(userName, password, ip);

            UserService.Verify(x => x.CheckUser(userName, password), Times.Once);
            SessionService.Verify(x => x.CreateSession(id, It.IsAny<int>(), ip), Times.Once);
            Assert.AreEqual(result.IsAuth, true);
            Assert.AreEqual(result.Ticket, ticket);
        }

        [TestMethod]
        public void Login_CheckUserFailed_ReturnFailed()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var userName = "userName1";
            var password = "password1";
            var ip = "127.0.0.1";
            var id = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var ticket = Guid.NewGuid().ToString();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow;
            var ExpiredDate = DateTime.UtcNow.AddMinutes(interval);
            var lastAccessDate = DateTime.UtcNow;

            UserService.Setup(x => x.CheckUser(userName, password)).Returns((Business.Models.User) null);

            SessionService.Setup(x => x.CreateSession(id, It.IsAny<int>(), ip)).Returns(new Session { Id = sessionId, UserId = id, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.Login(userName, password, ip);

            UserService.Verify(x => x.CheckUser(userName, password), Times.Once);
            SessionService.Verify(x => x.CreateSession(id, It.IsAny<int>(), ip), Times.Never);
            Assert.AreEqual(result.IsAuth, false);
            Assert.AreEqual(result.Ticket, null);
        }

        [TestMethod]
        public void CheckTokenWithoutRoles_TokenValid_ReturnOk()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = userId,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });
            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role>());

            UserService.Verify(x => x.GetUser(userId), Times.Once);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Never);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.SessionId, sessionId);
            Assert.AreEqual(result.Message, null);
        }

        [TestMethod]
        public void CheckTokenWithoutRoles_TokenValidNoRoles_ReturnOk()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = userId,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });
            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int>());

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role>());

            UserService.Verify(x => x.GetUser(userId), Times.Once);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Never);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.SessionId, sessionId);
            Assert.AreEqual(result.Message, null);
        }

        [TestMethod]
        public void CheckToken_TokenValid_ReturnOk()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new Business.Models.User
            {
                Id = userId,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });

            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUser(userId), Times.Once);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Once);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.SessionId, sessionId);
            Assert.AreEqual(result.Message, null);
        }

        [TestMethod]
        public void CheckToken_TokenNotFound_ReturnUnathorized()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ticket = Guid.NewGuid().ToString();

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns((Session) null);

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUserRoles(It.IsAny<Guid>()), Times.Never);
            UserService.Verify(x => x.GetUser(It.IsAny<Guid>()), Times.Never);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 401);
            Assert.AreEqual(result.UserId, null);
            Assert.AreEqual(result.SessionId, null);
            Assert.AreEqual(result.Message, null);
        }

        [TestMethod]
        public void CheckToken_SessionExpired_ReturnUnathorized()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(-100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var ticket = Guid.NewGuid().ToString();

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUserRoles(userId), Times.Never);
            UserService.Verify(x => x.GetUser(It.IsAny<Guid>()), Times.Never);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 401);
            Assert.AreEqual(result.UserId, null);
            Assert.AreEqual(result.SessionId, null);
            Assert.AreEqual(result.Message, "Session expired.");
        }

        [TestMethod]
        public void CheckToken_TokenValidUserIsBlocked_ReturnUnathorized()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var id = Guid.NewGuid();
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = id,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = false
            });

            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUserRoles(userId), Times.Never);
            UserService.Verify(x => x.GetUser(It.IsAny<Guid>()), Times.Once);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 401);
            Assert.AreEqual(result.UserId, null);
            Assert.AreEqual(result.SessionId, null);
            Assert.AreEqual(result.Message, "User not found or blocked.");
        }

        [TestMethod]
        public void CheckToken_TokenValidForbidenByRole_ReturnForbiden()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var id = Guid.NewGuid();
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = id,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });
            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.CheckToken(ticket, new List<Role> { Role.ManageTasks });

            UserService.Verify(x => x.GetUser(userId), Times.Once);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Once);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.StatusCode, 403);
            Assert.AreEqual(result.UserId, null);
            Assert.AreEqual(result.SessionId, null);
            Assert.AreEqual(result.Message, "Forbidden");
        }

        [TestMethod]
        public void Registrate_TokenValid_ShouldProlongSession()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = userId,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });
            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });
            SessionService.Setup(x => x.ProlongSession(sessionId)).Returns(true);

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.Registrate(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUser(userId), Times.Once);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Once);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            SessionService.Verify(x => x.ProlongSession(sessionId), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.SessionId, sessionId);
            Assert.AreEqual(result.Message, null);
        }

        [TestMethod]
        public void Registrate_TokenExpired_ShouldNotProlongSessionReturnUnathorized()
        {
            SessionService.ResetCalls();
            UserService.ResetCalls();

            var ip = "127.0.0.1";
            var id = Guid.NewGuid();
            var email = "email1@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var interval = 900;
            var sessionCreateDate = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate = DateTime.UtcNow.AddMinutes(-100);
            var lastAccessDate = DateTime.UtcNow.AddDays(-1);

            var roleId1 = 2;
            var roleId2 = 4;
            var roleId3 = 5;

            var ticket = Guid.NewGuid().ToString();

            UserService.Setup(x => x.GetUser(userId)).Returns(new User
            {
                Id = id,
                UserName = username,
                Email = email,
                Salt = salt,
                Password = secret,
                CreatedDate = createDate,
                UpdatedDate = updateDate,
                IsActive = true
            });
            UserService.Setup(x => x.GetUserRoles(userId)).Returns(new List<int> { roleId1, roleId2, roleId3 });

            SessionService.Setup(x => x.GetSessionByTicket(ticket)).Returns(new Session { Id = sessionId, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval, CreatedDate = sessionCreateDate, LastAccessDate = lastAccessDate, ExpiredDate = ExpiredDate });
            SessionService.Setup(x => x.ProlongSession(sessionId)).Returns(true);

            var service = new AuthService(SessionService.Object, UserService.Object);

            var result = service.Registrate(ticket, new List<Role> { Role.ManageSessions });

            UserService.Verify(x => x.GetUser(userId), Times.Never);
            UserService.Verify(x => x.GetUserRoles(userId), Times.Never);
            SessionService.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            SessionService.Verify(x => x.ProlongSession(sessionId), Times.Never);
            Assert.AreEqual(result.StatusCode, 401);
            Assert.AreEqual(result.UserId, null);
            Assert.AreEqual(result.SessionId, null);
            Assert.AreEqual(result.Message, "Session expired.");
        }
    }
}
