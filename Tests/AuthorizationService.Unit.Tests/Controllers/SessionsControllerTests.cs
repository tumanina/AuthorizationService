using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizationService.Api.Areas.V1.Controllers;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiWallet.UnitTests.ControllerTests
{
    [TestClass]
    public class SessionsControllerTests
    {
        private static readonly Mock<ISessionService> SessionService = new Mock<ISessionService>();

        [TestMethod]
        public void GetActive_SessionsExisted_ReturnOk()
        {
            SessionService.ResetCalls();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
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
            var expireDate1 = DateTime.UtcNow.AddDays(1);
            var expireDate2 = DateTime.UtcNow.AddDays(2);
            var expireDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            SessionService.Setup(x => x.GetActiveSessions()).Returns(new List<Session>
            {
                new Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = expireDate1 },
                new Session { Id = id2, UserId = userId2, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = expireDate2 },
                new Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = expireDate3 }
            });

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.GetActive();
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as IEnumerable<AuthorizationService.Api.Areas.V1.Models.Session>;

            SessionService.Verify(x => x.GetActiveSessions(), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.Count(), 3);
            Assert.IsTrue(pagedResult.Any(t => t.Id == id1 && t.UserId == userId1 && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == expireDate1));
            Assert.IsTrue(pagedResult.Any(t => t.Id == id2 && t.UserId == userId2 && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == expireDate2));
            Assert.IsTrue(pagedResult.Any(t => t.Id == id3 && t.UserId == userId3 && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == expireDate3));
        }

        [TestMethod]
        public void GetActive_SessionsNotExisted_ReturnCorrect()
        {
            SessionService.ResetCalls();

            SessionService.Setup(x => x.GetActiveSessions()).Returns(new List<Session>());

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.GetActive();
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as IEnumerable<AuthorizationService.Api.Areas.V1.Models.Session>;

            SessionService.Verify(x => x.GetActiveSessions(), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.Count(), 0);
        }

        [TestMethod]
        public void GetSession_SessionExisted_ReturnOk()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            SessionService.Setup(x => x.GetSession(id)).Returns(new Session { Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval } );

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var session = result.Value as AuthorizationService.Api.Areas.V1.Models.Session;

            SessionService.Verify(x => x.GetSession(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(session.UserId, userId);
            Assert.AreEqual(session.UpdateExpireInc, interval);
            Assert.AreEqual(session.IP, ip);
            Assert.AreEqual(session.Id, id);
        }
        
        [TestMethod]
        public void GetSession_SessionNotExisted_ReturnNotFound()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();

            SessionService.Setup(x => x.GetSession(id)).Returns((Session) null);

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            SessionService.Verify(x => x.GetSession(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetSession_ServiceReturnException_ReturnInternalServerError()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();
            var exceptionMessage = "some exception message";

            SessionService.Setup(x => x.GetSession(id)).Throws(new Exception(exceptionMessage));

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            SessionService.Verify(x => x.GetSession(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void CloseSession_SessionExisted_ReturnCorrect()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            var session = new Session { Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval };

            SessionService.Setup(x => x.CloseSession(id)).Returns(session);

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.CloseSession(id);

            var result = actionResult as OkObjectResult;
            var closedSession = result.Value as AuthorizationService.Api.Areas.V1.Models.Session;

            SessionService.Verify(x => x.CloseSession(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(closedSession.UserId, userId);
            Assert.AreEqual(closedSession.UpdateExpireInc, interval);
            Assert.AreEqual(closedSession.IP, ip);
            Assert.AreEqual(closedSession.Id, id);
        }

        [TestMethod]
        public void CloseSession_SessionNotExisted_ReturnCorrect()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();

            SessionService.Setup(x => x.CloseSession(id)).Returns((Session)null);

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.CloseSession(id);

            var result = actionResult as NotFoundResult;

            SessionService.Verify(x => x.CloseSession(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void CloseSession_ServiceReturnException_ReturnInternalServerError()
        {
            SessionService.ResetCalls();

            var id = Guid.NewGuid();

            var exceptionMessage = "some exception message";

            SessionService.Setup(x => x.CloseSession(id)).Throws(new Exception(exceptionMessage));

            var controller = new SessionsController(SessionService.Object);

            var actionResult = controller.CloseSession(id);

            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            SessionService.Verify(x => x.CloseSession(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }
    }
}
