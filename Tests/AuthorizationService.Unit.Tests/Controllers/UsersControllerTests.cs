using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AuthorizationService.Api.Areas.V1.Controllers;
using AuthorizationService.Api.Areas.V1.Models;
using AuthorizationService.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Session = AuthorizationService.Business.Models.Session;
using User = AuthorizationService.Business.Models.User;

namespace AuthorizationService.Unit.Tests.ControllerTests
{
    [TestClass]
    public class UsersControllerTests
    {
        private static readonly Mock<IUserService> UserService = new Mock<IUserService>();

        [TestMethod]
        public void GetUserById_UserExisted_ReturnOk()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name2";
            var salt = "22ythgdh9";
            var secret = "111gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-2);
            var updateDate = DateTime.UtcNow.AddMinutes(-6);

            UserService.Setup(x => x.GetUser(id)).Returns(new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true } );

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var user = result.Value as AuthorizationService.Api.Areas.V1.Models.User;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(user.UserName, username);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.CreatedDate, createDate);
            Assert.AreEqual(user.UpdatedDate, updateDate);
            Assert.AreEqual(user.IsActive, true);
            Assert.AreEqual(user.Id, id);
        }
        
        [TestMethod]
        public void GetUserById_UserNotExisted_ReturnNotFound()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            UserService.Setup(x => x.GetUser(id)).Returns((User) null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetUserById_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var exceptionMessage = "some exception message";

            UserService.Setup(x => x.GetUser(id)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetUserByName_UserExisted_ReturnOk()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name2";
            var salt = "22ythgdh9";
            var secret = "111gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-2);
            var updateDate = DateTime.UtcNow.AddMinutes(-6);

            UserService.Setup(x => x.GetUserByName(username)).Returns(new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(username);
            var result = actionResult as OkObjectResult;
            var user = result.Value as Api.Areas.V1.Models.User;

            UserService.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(user.UserName, username);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.CreatedDate, createDate);
            Assert.AreEqual(user.UpdatedDate, updateDate);
            Assert.AreEqual(user.IsActive, true);
            Assert.AreEqual(user.Id, id);
        }

        [TestMethod]
        public void GetUserByName_UserNotExisted_ReturnNotFound()
        {
            UserService.ResetCalls();

            var username = "name2";

            UserService.Setup(x => x.GetUserByName(username)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(username);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetUserByName_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var username = "name2";
            var exceptionMessage = "some exception message";

            UserService.Setup(x => x.GetUserByName(username)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(username);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.GetUserByName(username), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetUserByEmail_UserExisted_ReturnOk()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name2";
            var salt = "22ythgdh9";
            var secret = "111gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-2);
            var updateDate = DateTime.UtcNow.AddMinutes(-6);

            UserService.Setup(x => x.GetUserByEmail(email)).Returns(new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(null, email);
            var result = actionResult as OkObjectResult;
            var user = result.Value as AuthorizationService.Api.Areas.V1.Models.User;

            UserService.Verify(x => x.GetUserByEmail(email), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(user.UserName, username);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.CreatedDate, createDate);
            Assert.AreEqual(user.UpdatedDate, updateDate);
            Assert.AreEqual(user.IsActive, true);
            Assert.AreEqual(user.Id, id);
        }

        [TestMethod]
        public void GetUserByEmail_UserNotExisted_ReturnNotFound()
        {
            UserService.ResetCalls();

            var email = "email2@mail.ru";

            UserService.Setup(x => x.GetUserByEmail(email)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(null, email);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.GetUserByEmail(email), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetUserByEmail_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var email = "email2@mail.ru";
            var exceptionMessage = "some exception message";

            UserService.Setup(x => x.GetUserByEmail(email)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Get(null, email);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.GetUserByEmail(email), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetRolesByUserId_UserExisted_ReturnOk()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name2";
            var salt = "22ythgdh9";
            var secret = "111gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-2);
            var updateDate = DateTime.UtcNow.AddMinutes(-6);

            UserService.Setup(x => x.GetUser(id)).Returns(new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var user = result.Value as AuthorizationService.Api.Areas.V1.Models.User;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(user.UserName, username);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.CreatedDate, createDate);
            Assert.AreEqual(user.UpdatedDate, updateDate);
            Assert.AreEqual(user.IsActive, true);
            Assert.AreEqual(user.Id, id);
        }

        [TestMethod]
        public void GetRolesByUserId_UserNotExisted_ReturnNotFound()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            UserService.Setup(x => x.GetUser(id)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetRolesByUserId_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var exceptionMessage = "some exception message";

            UserService.Setup(x => x.GetUser(id)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.GetUser(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetAllUserSessions_SessionsExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId = Guid.NewGuid();
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

            var data = new List<Session>
            {
                new Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            UserService.Setup(x => x.GetSessions(userId, false)).Returns(data);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetSessionsById(userId, false);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, false), Times.Once);
            Assert.AreEqual(sessions.Count(), 3);
            Assert.IsTrue(sessions.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(sessions.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(sessions.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetAllUserSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var userId = Guid.NewGuid();

            var data = new List<Session>();

            UserService.Setup(x => x.GetSessions(userId, false)).Returns(data);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetSessionsById(userId, false);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, false), Times.Once);
            Assert.AreEqual(sessions.Count(), 0);
        }

        [TestMethod]
        public void GetActiveUserSessions_SessionsExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId = Guid.NewGuid();
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

            var data = new List<Session>
            {
                new Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            UserService.Setup(x => x.GetSessions(userId, true)).Returns(data);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetSessionsById(userId);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, true), Times.Once);
            Assert.AreEqual(sessions.Count(), 3);
            Assert.IsTrue(sessions.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(sessions.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(sessions.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetActiveUserSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var userId= Guid.NewGuid();

            var data = new List<Session>();

            UserService.Setup(x => x.GetSessions(userId, true)).Returns(data);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.GetSessionsById(userId, true);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, true), Times.Once);
            Assert.AreEqual(sessions.Count(), 0);
        }

        [TestMethod]
        public void GetOwnSessions_SessionsExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId = Guid.NewGuid();
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

            var data = new List<Session>
            {
                new Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            UserService.Setup(x => x.GetSessions(userId, false)).Returns(data);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Items.Add("userId", userId);

            var actionResult = controller.GetOwnSessions(false);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, false), Times.Once);
            Assert.AreEqual(sessions.Count(), 3);
            Assert.IsTrue(sessions.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(sessions.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(sessions.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetOwnSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var userId= Guid.NewGuid();

            var data = new List<Session>();

            UserService.Setup(x => x.GetSessions(userId, false)).Returns(data);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Items.Add("userId", userId);

            var actionResult = controller.GetOwnSessions(false);
            var result = actionResult as OkObjectResult;
            var sessions = result.Value as IEnumerable<Api.Areas.V1.Models.Session>;

            UserService.Verify(x => x.GetSessions(userId, false), Times.Once);
            Assert.AreEqual(sessions.Count(), 0);
        }

        [TestMethod]
        public void GetOwnSessions_UserNotAuthorised_ShouldReturnCorrect()
        {
            UserService.ResetCalls();

            var userId= Guid.NewGuid();

            var data = new List<Session>();

            UserService.Setup(x => x.GetSessions(userId, false)).Returns(data);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var actionResult = controller.GetOwnSessions(false);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as UnauthorizedResult;

            UserService.Verify(x => x.GetSessions(userId, false), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 401);
        }

        [TestMethod]
        public void CreateUser_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var createdUsername = string.Empty;
            var createdEmail = string.Empty;
            var createdPassword = string.Empty;

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user)
            .Callback((string emailParam, string usernameParam, string passwordParam) => { createdEmail = emailParam; createdUsername = usernameParam; createdPassword = passwordParam; });

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                UserName = username,
                Password = password
            });

            var result = actionResult as CreatedResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Once);
            Assert.AreEqual(result.StatusCode, 201);
            Assert.AreEqual(createdEmail, email);
            Assert.AreEqual(createdUsername, username);
            Assert.AreEqual(createdPassword, password);
        }

        [TestMethod]
        public void CreateUser_RequestIsEmpty_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var createdUsername = string.Empty;
            var createdEmail = string.Empty;
            var createdPassword = string.Empty;

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user)
            .Callback((string emailParam, string usernameParam, string passwordParam) => { createdEmail = emailParam; createdUsername = usernameParam; createdPassword = passwordParam; });

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/users";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(null);

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        public void CreateUser_UserNameIsEmpty_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);


            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        public void CreateUser_UserNameTooLong_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1dsdgchrhtfjngjmfgvjdrhturbvncfghuru";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                UserName = username,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        public void CreateUser_PasswordIsEmpty_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);


            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                UserName = username
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        public void CreateUser_EmailIsEmpty_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);


            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                UserName = username,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        public void CreateUser_EmailHasInvalidFormat_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email";
            var username = "name";
            var password = "123654";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);
            
            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.CreateUser(email, username, password)).Returns(user);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/nodes";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                UserName = username,
                Email = email,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        [TestMethod]
        public void CreateUser_ServiceReturnNull_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";
            
            UserService.Setup(x => x.CreateUser(email, username, password)).Returns((User)null);

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/users";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                UserName = username,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void CreateUser_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var email = "email2@mail.ru";
            var username = "name1";
            var password = "123654";

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.CreateUser(email, username, password)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "http";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("someurl.com", 72001);
            controller.ControllerContext.HttpContext.Request.Path = "/api/v1/users";
            controller.ControllerContext.HttpContext.Request.Method = HttpMethod.Post.ToString();

            var actionResult = controller.Post(new CreateUserRequest
            {
                Email = email,
                UserName = username,
                Password = password
            });

            var result = actionResult as CreatedResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.CreateUser(email, username, password), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void UpdateUser_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var createdUsername = string.Empty;
            var createdEmail = string.Empty;

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.UpdateUser(id, email, username)).Returns(user)
            .Callback((Guid idParam, string emailParam, string usernameParam) => { createdEmail = emailParam; createdUsername = usernameParam; });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, new UpdateUserRequest
            {
                Email = email,
                UserName = username
            });

            var result = actionResult as OkObjectResult;

            UserService.Verify(x => x.UpdateUser(id, email, username), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(createdEmail, email);
            Assert.AreEqual(createdUsername, username);
        }

        [TestMethod]
        public void UpdateUser_RequestIsEmpty_ReturnBadRequest()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var createdUsername = string.Empty;
            var createdEmail = string.Empty;
            var createdPassword = string.Empty;

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = true };

            UserService.Setup(x => x.UpdateUser(id, email, username)).Returns(user)
            .Callback((string emailParam, string usernameParam, string passwordParam) => { createdEmail = emailParam; createdUsername = usernameParam; createdPassword = passwordParam; });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, (UpdateUserRequest) null);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as BadRequestObjectResult;

            UserService.Verify(x => x.UpdateUser(id, email, username), Times.Never);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 400);
        }

        [TestMethod]
        public void UpdateUser_ServiceReturnNull_ReturnNotFound()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";

            UserService.Setup(x => x.UpdateUser(id, email, username)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, new UpdateUserRequest
            {
                Email = email,
                UserName = username
            });

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.UpdateUser(id, email, username), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 404);
        }

        [TestMethod]
        public void UpdateUser_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.UpdateUser(id, email, username)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, new UpdateUserRequest
            {
                Email = email,
                UserName = username
            });

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.UpdateUser(id, email, username), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void ChangePassword_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var password = "123654";

            var newPassword = string.Empty;
            
            UserService.Setup(x => x.ChangePassword(id, password)).Returns(true)
            .Callback((Guid idParam, string passwordParam) => { newPassword = passwordParam; });

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, password);

            var result = actionResult as OkObjectResult;

            UserService.Verify(x => x.ChangePassword(id, password), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, true);
            Assert.AreEqual(newPassword, password);
        }

        [TestMethod]
        public void ChangePassword_ServiceReturnFalse_ReturnOkAndFalse()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var password = "123654";

            UserService.Setup(x => x.ChangePassword(id, password)).Returns(false);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, password);

            var result = actionResult as OkObjectResult;

            UserService.Verify(x => x.ChangePassword(id, password), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, false);
        }

        [TestMethod]
        public void ChangePassword_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var password = "123654";

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.ChangePassword(id, password)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Put(id, password);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.ChangePassword(id, password), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void BlockUser_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = false };

            UserService.Setup(x => x.UpdateActive(id, false)).Returns(user);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Block(id);

            var result = actionResult as OkObjectResult;

            UserService.Verify(x => x.UpdateActive(id, false), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void BlockUser_ServiceReturnNull_ReturnNotFound()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            UserService.Setup(x => x.UpdateActive(id, false)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Block(id);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.UpdateActive(id, false), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 404);
        }

        [TestMethod]
        public void BlockUser_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.UpdateActive(id, false)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.Block(id);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.UpdateActive(id, false), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void UnblockUser_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var email = "email2@mail.ru";
            var username = "name1";
            var salt = "54ythgdh9";
            var secret = "542gythnfsli8";
            var createDate = DateTime.UtcNow.AddDays(-1);
            var updateDate = DateTime.UtcNow.AddMinutes(-3);

            var user = new User { Id = id, UserName = username, Email = email, Salt = salt, Password = secret, CreatedDate = createDate, UpdatedDate = updateDate, IsActive = false };

            UserService.Setup(x => x.UpdateActive(id, true)).Returns(user);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.UnBlock(id);

            var result = actionResult as OkObjectResult;

            UserService.Verify(x => x.UpdateActive(id, true), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void UnblockUser_ServiceReturnNull_ReturnNotFound()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            UserService.Setup(x => x.UpdateActive(id, true)).Returns((User)null);

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.UnBlock(id);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            UserService.Verify(x => x.UpdateActive(id, true), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 404);
        }

        [TestMethod]
        public void UnblockUser_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.UpdateActive(id, true)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.UnBlock(id);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.UpdateActive(id, true), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }

        [TestMethod]
        public void SetRoles_Success_ReturnCreatedAndCorrect()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var roles = new List<int> { 2,4,5 };

            UserService.Setup(x => x.UpdateRoles(id, roles));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.SetRoles(id, roles);

            var result = actionResult as OkResult;

            UserService.Verify(x => x.UpdateRoles(id, roles), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void SetRoles_ServiceReturnException_ReturnInternalServerError()
        {
            UserService.ResetCalls();

            var id = Guid.NewGuid();
            var roles = new List<int> { 2, 4, 5 };

            var exceptionMessage = "any exception message";

            UserService.Setup(x => x.UpdateRoles(id, roles)).Throws(new Exception(exceptionMessage));

            var controller = new UsersController(UserService.Object);

            var actionResult = controller.SetRoles(id, roles);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            UserService.Verify(x => x.UpdateRoles(id, roles), Times.Once);
            Assert.AreEqual(result, null);
            Assert.AreEqual(result1.StatusCode, 500);
        }
    }
}