using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AuthorizationService.Api.Areas.V1.Controllers;
using AuthorizationService.Api.Areas.V1.Models;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Unit.Tests.ControllerTests
{
    [TestClass]
    public class LoginControllerTests
    {
        private static readonly Mock<IAuthService> AuthService = new Mock<IAuthService>();
        private static readonly Mock<ILogger<LoginController>> Logger = new Mock<ILogger<LoginController>>();

        [TestMethod]
        public void Login_AuthSuccess_ReturnOkTicket()
        {
            AuthService.Invocations.Clear();

            var userName = "name1";
            var password = "password1";
            var ip = "127.0.0.5";
            var ticket = Guid.NewGuid().ToString();

            AuthService.Setup(x => x.Login(userName, password, ip)).Returns(new LoginResult { IsAuth  = true, Ticket = ticket });

            var controller = new LoginController(AuthService.Object, Logger.Object);

            var actionResult = controller.Login(new LoginRequest { UserName = userName, Password = password, IP = ip });
            var result = actionResult as OkObjectResult;

            AuthService.Verify(x => x.Login(userName, password, ip), Times.Once);
            Assert.AreEqual(result.Value, ticket);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void Login_AuthFailed_ReturnUnauthorized()
        {
            AuthService.Invocations.Clear();

            var userName = "name1";
            var password = "password1";
            var ip = "127.0.0.5";

            AuthService.Setup(x => x.Login(userName, password, ip)).Returns(new LoginResult { IsAuth = false });

            var controller = new LoginController(AuthService.Object, Logger.Object);

            var actionResult = controller.Login(new LoginRequest { UserName = userName, Password = password, IP = ip });
            var result = actionResult as OkObjectResult;

            var result1 = actionResult as ObjectResult;

            AuthService.Verify(x => x.Login(userName, password, ip), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 401);
        }

        [TestMethod]
        public void Login_RequestNull_ReturnBadRequest()
        {
            AuthService.Invocations.Clear();

            var userName = "name1";
            var password = "password1";
            var ip = "127.0.0.5";

            AuthService.Setup(x => x.Login(userName, password, ip)).Returns(new LoginResult { IsAuth = false });

            var controller = new LoginController(AuthService.Object, Logger.Object);

            var actionResult = controller.Login((LoginRequest) null);

            var result = actionResult as OkObjectResult;

            var result1 = actionResult as BadRequestObjectResult;

            AuthService.Verify(x => x.Login(userName, password, ip), Times.Never);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void Login_IpAddressHasInvalidFormat_ReturnBadRequest()
        {
            AuthService.Invocations.Clear();

            var userName = "name1";
            var password = "password1";
            var ip = "127";
            var ticket = Guid.NewGuid().ToString();

            AuthService.Setup(x => x.Login(userName, password, ip)).Returns(new LoginResult { IsAuth = true, Ticket = ticket });

            var controller = new LoginController(AuthService.Object, Logger.Object);

            var actionResult = controller.Login(new LoginRequest { UserName = userName, Password = password, IP = ip });
            var result = actionResult as OkObjectResult;

            var result1 = actionResult as BadRequestObjectResult;

            AuthService.Verify(x => x.Login(userName, password, ip), Times.Never);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void Login_ServiceReturnException_ReturnInternalServerError()
        {
            AuthService.Invocations.Clear();

            var userName = "name1";
            var password = "password1";
            var ip = "127.0.0.5";
            var exceptionMessage = "some exception message";

            AuthService.Setup(x => x.Login(userName, password, ip)).Throws(new Exception(exceptionMessage));

            var controller = new LoginController(AuthService.Object, Logger.Object);

            var actionResult = controller.Login(new LoginRequest { UserName = userName, Password = password, IP = ip });

            var result = actionResult as OkObjectResult;

            var result1 = actionResult as ObjectResult;

            AuthService.Verify(x => x.Login(userName, password, ip), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }
    }
}
