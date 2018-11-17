using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AuthorizationService.Business;
using AuthorizationService.Repositories;

namespace AuthorizationService.Unit.Tests.ServiceTests
{
    [TestClass]
    public class SessionServiceTest
    {
        private static readonly Mock<ISessionRepository> SessionRepository = new Mock<ISessionRepository>();

        [TestMethod]
        public void GetActiveSessions_SessionsExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

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
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate2 = DateTime.UtcNow.AddDays(2);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id2, UserId = userId2, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            };

            SessionRepository.Setup(x => x.GetActiveSessions(null)).Returns(data);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetActiveSessions();

            SessionRepository.Verify(x => x.GetActiveSessions(null), Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId1 && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId2 && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId3 && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetActiveSessions_SessionsNotExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var data = new List<Repositories.Entities.Session>();

            SessionRepository.Setup(x => x.GetActiveSessions(null)).Returns(data);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetActiveSessions();

            SessionRepository.Verify(x => x.GetActiveSessions(null), Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetSession_SessionExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            var session = new Repositories.Entities.Session {Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval};

            SessionRepository.Setup(x => x.GetSession(id)).Returns(session);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetSession(id);

            SessionRepository.Verify(x => x.GetSession(id), Times.Once);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.UpdateExpireInc, interval);
            Assert.AreEqual(result.IP, ip);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetSession_SessionNotExisted_ShouldReturnNull()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();

            SessionRepository.Setup(x => x.GetSession(id)).Returns((Repositories.Entities.Session)null);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetSession(id);

            SessionRepository.Verify(x => x.GetSession(id), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetSessionByTicket_SessionExisted_ShouldReturnCorrect()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var ticket = Guid.NewGuid().ToString();
            var interval = 1100;
            var ip = "127.0.0.3";

            var session = new Repositories.Entities.Session { Id = id, UserId = userId, IP = ip, Ticket = ticket, UpdateExpireInc = interval };

            SessionRepository.Setup(x => x.GetSessionByTicket(ticket)).Returns(session);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetSessionByTicket(ticket);

            SessionRepository.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.UpdateExpireInc, interval);
            Assert.AreEqual(result.IP, ip);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetSessionByTicket_SessionNotExisted_ShouldReturnNull()
        {
            SessionRepository.ResetCalls();

            var ticket = Guid.NewGuid().ToString();

            SessionRepository.Setup(x => x.GetSessionByTicket(ticket)).Returns((Repositories.Entities.Session)null);

            var service = new SessionService(SessionRepository.Object);

            var result = service.GetSessionByTicket(ticket);

            SessionRepository.Verify(x => x.GetSessionByTicket(ticket), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CreateSession_Success_ShouldReturnSession()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            var entity = new Repositories.Entities.Session { Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval };

            SessionRepository.Setup(x => x.CreateSession(userId, interval, ip)).Returns(entity);

            var service = new SessionService(SessionRepository.Object);

            var result = service.CreateSession(userId, interval, ip);

            SessionRepository.Verify(x => x.CreateSession(userId, interval, ip), Times.Once);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.UpdateExpireInc, interval);
            Assert.AreEqual(result.IP, ip);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void CreateSession_ServiceReturnNull_ShouldReturnNull()
        {
            SessionRepository.ResetCalls();

            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            SessionRepository.Setup(x => x.CreateSession(userId, interval, ip)).Returns((Repositories.Entities.Session)null);

            var service = new SessionService(SessionRepository.Object);

            var result = service.CreateSession(userId, interval, ip);

            SessionRepository.Verify(x => x.CreateSession(userId, interval, ip), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void ProlongSession_Success_ShouldReturnSession()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();

            var sessionId = Guid.NewGuid();

            SessionRepository.Setup(x => x.ProlongSession(id)).Returns(true).Callback((Guid idParam) => { sessionId = idParam; });

            var service = new SessionService(SessionRepository.Object);

            var result = service.ProlongSession(id);

            SessionRepository.Verify(x => x.ProlongSession(id), Times.Once);
            Assert.AreEqual(result, true);
            Assert.AreEqual(sessionId, id);
        }

        [TestMethod]
        public void CloseSession_Success_ShouldReturnSession()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            var entity = new Repositories.Entities.Session { Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval };

            SessionRepository.Setup(x => x.CloseSession(id)).Returns(entity);

            var service = new SessionService(SessionRepository.Object);

            var result = service.CloseSession(id);

            SessionRepository.Verify(x => x.CloseSession(id), Times.Once);
            Assert.AreEqual(result.UserId, userId);
            Assert.AreEqual(result.UpdateExpireInc, interval);
            Assert.AreEqual(result.IP, ip);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void CloseSession_ServiceReturnNull_ShouldReturnNull()
        {
            SessionRepository.ResetCalls();

            var id = Guid.NewGuid();

            SessionRepository.Setup(x => x.CloseSession(id)).Returns((Repositories.Entities.Session)null);

            var service = new SessionService(SessionRepository.Object);

            var result = service.CloseSession(id);

            SessionRepository.Verify(x => x.CloseSession(id), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CloseUserSessions_SessionsExisted_ShouldReturnSession()
        {
            SessionRepository.ResetCalls();

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
            var ExpiredDate1 = DateTime.UtcNow.AddSeconds(-1);
            var ExpiredDate2 = DateTime.UtcNow.AddSeconds(-1);
            var ExpiredDate3 = DateTime.UtcNow.AddSeconds(-1);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id2, UserId = userId, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Repositories.Entities.Session { Id = id3, UserId = userId, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 },
            };

            SessionRepository.Setup(x => x.CloseSessions(userId)).Returns(data);

            var service = new SessionService(SessionRepository.Object);

            var result = service.CloseSessions(userId);

            SessionRepository.Verify(x => x.CloseSessions(userId), Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void CloseUserSessions_SessionsNotExisted_ShouldReturnEmptyList()
        {
            SessionRepository.ResetCalls();

            var userId = Guid.NewGuid();

            SessionRepository.Setup(x => x.CloseSessions(userId)).Returns(new List<Repositories.Entities.Session>());

            var service = new SessionService(SessionRepository.Object);

            var result = service.CloseSessions(userId);

            SessionRepository.Verify(x => x.CloseSessions(userId), Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }
    }
}
