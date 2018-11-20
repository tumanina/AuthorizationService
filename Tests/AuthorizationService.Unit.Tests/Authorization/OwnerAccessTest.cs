using System;
using System.Collections.Generic;
using AuthorizationService.Api.Authorization;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace AuthorizationService.Unit.Tests.Authorization
{
    [TestClass]
    public class OwnerAccessTest
    {
        private static readonly Mock<IAuthService> AuthService = new Mock<IAuthService>();
        
        [TestMethod]
        public void OnAuthorization_TokenExistAndValid_Correct()
        {
            AuthService.Invocations.Clear();

            var userid = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var token = Guid.NewGuid().ToString();
            
            AuthService.Setup(x => x.Registrate(token, new List<Role>())).Returns(new AuthResult { StatusCode = 200, UserId = userid, SessionId = sessionId });
            var actionDescriptor = new Mock<ControllerActionDescriptor>();

            var filter = new OwnerAccessAuthorizationFilter(AuthService.Object);

            var controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext(), RouteData = new RouteData(), ActionDescriptor = actionDescriptor.Object};
            controllerContext.HttpContext.Request.Headers["Authorization"] = token;

            var context = new AuthorizationFilterContext(controllerContext, new List<IFilterMetadata>());

            filter.OnAuthorization(context);

            AuthService.Verify(x => x.Registrate(token, new List<Role>()), Times.Once);
            var result = context.HttpContext.Items.TryGetValue("userId", out var contextUserId);
            Assert.AreEqual(result, true);
            Assert.AreEqual(contextUserId, userid); 
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 200);
            Assert.AreEqual(context.Result, null);
        }

        [TestMethod]
        public void OnAuthorization_TokenNotSpecified_ReturnUnauthorized()
        {
            AuthService.Invocations.Clear();

            var userid = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var token = Guid.NewGuid().ToString();

            AuthService.Setup(x => x.Registrate(token, new List<Role>())).Returns(new AuthResult { StatusCode = 200, UserId = userid, SessionId = sessionId });
            var actionDescriptor = new Mock<ControllerActionDescriptor>();

            var filter = new OwnerAccessAuthorizationFilter(AuthService.Object);

            var controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext(), RouteData = new RouteData(), ActionDescriptor = actionDescriptor.Object };

            var context = new AuthorizationFilterContext(controllerContext, new List<IFilterMetadata>());

            filter.OnAuthorization(context);

            AuthService.Verify(x => x.Registrate(token, new List<Role>()), Times.Never);
            var result = context.HttpContext.Items.TryGetValue("userId", out var contextUserId);
            Assert.AreEqual(result, false);
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 401);
            Assert.IsTrue(context.Result is UnauthorizedResult);
        }

        [TestMethod]
        public void OnAuthorization_ServiceReturnUnauthorized_ReturnUnauthorized()
        {
            AuthService.Invocations.Clear();

            var token = Guid.NewGuid().ToString();
            var authMessage = "some unauthorized error";

            AuthService.Setup(x => x.Registrate(token, new List<Role>())).Returns(new AuthResult { StatusCode = 401, Message = authMessage });
            var actionDescriptor = new Mock<ControllerActionDescriptor>();

            var filter = new OwnerAccessAuthorizationFilter(AuthService.Object);

            var controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext(), RouteData = new RouteData(), ActionDescriptor = actionDescriptor.Object };
            controllerContext.HttpContext.Request.Headers["Authorization"] = token;

            var context = new AuthorizationFilterContext(controllerContext, new List<IFilterMetadata>());

            filter.OnAuthorization(context);

            AuthService.Verify(x => x.Registrate(token, new List<Role>()), Times.Once);
            var result = context.HttpContext.Items.TryGetValue("userId", out var contextUserId);

            Assert.AreEqual(result, false);
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 401);
            Assert.IsTrue(context.Result is UnauthorizedResult);
        }

        [TestMethod]
        public void OnAuthorization_ServiceReturnForbidden_ReturnForbidden()
        {
            AuthService.Invocations.Clear();

            var token = Guid.NewGuid().ToString();
            var authMessage = "some unauthorized error";

            AuthService.Setup(x => x.Registrate(token, new List<Role>())).Returns(new AuthResult { StatusCode = 403, Message = authMessage });
            var actionDescriptor = new Mock<ControllerActionDescriptor>();

            var filter = new OwnerAccessAuthorizationFilter(AuthService.Object);

            var controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext(), RouteData = new RouteData(), ActionDescriptor = actionDescriptor.Object };
            controllerContext.HttpContext.Request.Headers["Authorization"] = token;

            var context = new AuthorizationFilterContext(controllerContext, new List<IFilterMetadata>());

            filter.OnAuthorization(context);

            AuthService.Verify(x => x.Registrate(token, new List<Role>()), Times.Once);
            var result = context.HttpContext.Items.TryGetValue("userId", out var contextUserId);

            Assert.AreEqual(result, false);
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 403);
            Assert.IsTrue(context.Result is StatusCodeResult);
            Assert.AreEqual((context.Result as StatusCodeResult).StatusCode, 403);
        }
    }
}
