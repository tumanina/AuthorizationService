using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AuthorizationService.Repositories;
using AuthorizationService.Repositories.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationService.Unit.Tests.RepositoryTests
{
    [TestClass]
    public class SessionRepositoryTest
    {
        private static readonly Mock<IAuthDBContext> AuthDBContext = new Mock<IAuthDBContext>();
        private static readonly Mock<IAuthDBContextFactory> AuthDBContextFactory = new Mock<IAuthDBContextFactory>();

        [TestMethod]
        public void GetAllActiveSessions_SessionsExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetActiveSessions();

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 3);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId1 && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId2 && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId3 && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetAllActiveSessions_SessionsNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            var data = new List<Repositories.Entities.Session>().AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetActiveSessions();

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetActiveSessionsByUser_ActiveSessionsByUserExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetActiveSessions(userId3);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 1);
            Assert.IsTrue(result.Any(t => t.Id == id3 && t.UserId == userId3 && t.Ticket == ticket3 && t.UpdateExpireInc == interval3 && t.CreatedDate == createDate3 && t.LastAccessDate == lastAccessDate3 && t.ExpiredDate == ExpiredDate3));
        }

        [TestMethod]
        public void GetActiveSessionsByUser_SessionsByUserNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetActiveSessions(Guid.NewGuid());

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetUserSessions_UserSessionsExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
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
                new Repositories.Entities.Session { Id = id2, UserId = userId1, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Repositories.Entities.Session { Id = id3, UserId = userId2, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSessions(userId1);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result.Any(t => t.Id == id1 && t.UserId == userId1 && t.Ticket == ticket1 && t.UpdateExpireInc == interval1 && t.CreatedDate == createDate1 && t.LastAccessDate == lastAccessDate1 && t.ExpiredDate == ExpiredDate1));
            Assert.IsTrue(result.Any(t => t.Id == id2 && t.UserId == userId1 && t.Ticket == ticket2 && t.UpdateExpireInc == interval2 && t.CreatedDate == createDate2 && t.LastAccessDate == lastAccessDate2 && t.ExpiredDate == ExpiredDate2));
        }

        [TestMethod]
        public void GetUserSessionsByUser_UserSessionsNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSessions(Guid.NewGuid());

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public void GetSession_SessionExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSession(id2);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.UserId, userId2);
            Assert.AreEqual(result.Ticket, ticket2);
            Assert.AreEqual(result.IP, ip2);
            Assert.AreEqual(result.UpdateExpireInc, interval2);
            Assert.AreEqual(result.CreatedDate, createDate2);
            Assert.AreEqual(result.ExpiredDate, ExpiredDate2);
            Assert.AreEqual(result.LastAccessDate, lastAccessDate2);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetSession_SessionNotExisted_UseDbContextReturnNull()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSession(id2);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetSessionByTicket_SessionExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

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
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSessionByTicket(ticket2);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result.UserId, userId2);
            Assert.AreEqual(result.Ticket, ticket2);
            Assert.AreEqual(result.IP, ip2);
            Assert.AreEqual(result.UpdateExpireInc, interval2);
            Assert.AreEqual(result.CreatedDate, createDate2);
            Assert.AreEqual(result.ExpiredDate, ExpiredDate2);
            Assert.AreEqual(result.LastAccessDate, lastAccessDate2);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetSessionByTicket_SessionNotExisted_UseDbContextReturnNull()
        {
            AuthDBContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            var id1 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket2 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.GetSessionByTicket(ticket2);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CreateSession_Correct_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var interval = 1100;
            var ip = "127.0.0.3";

            var session = new Repositories.Entities.Session();

            var mockSet0 = new Mock<DbSet<Repositories.Entities.Session>>();

            var data0 = new List<Repositories.Entities.Session>().AsQueryable();

            mockSet0.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data0.Provider);
            mockSet0.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data0.Expression);
            mockSet0.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data0.ElementType);
            mockSet0.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data0.GetEnumerator());
            mockSet0.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()))
                .Callback((Repositories.Entities.Session sessionParam) => { session = sessionParam; });

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id, UserId = userId, IP = ip, UpdateExpireInc = interval }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet0.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.CreateSession(userId, interval, ip);

            AuthDBContext.Verify(x => x.UserSession, Times.Once);
            AuthDBContext.Verify(x => x.Set<Repositories.Entities.Session>(), Times.Once);
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            mockSet0.Verify(x => x.Add(It.IsAny<Repositories.Entities.Session>()), Times.Once);
            Assert.AreEqual(session.UserId, userId);
            Assert.AreEqual(session.UpdateExpireInc, interval);
            Assert.AreEqual(session.IP, ip);
        }

        [TestMethod]
        public void ProlongSession_SessionExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

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
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.ProlongSession(id1);

            Assert.IsTrue(result);
            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void ProlongSession_SessionNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.ProlongSession(id2);

            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void CloseSession_SessionExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

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
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.CloseSession(id1);

            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(2));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.IsTrue(result.ExpiredDate< DateTime.UtcNow);
            Assert.AreEqual(result.Id, id1);
        }

        [TestMethod]
        public void CloseSession_SessionNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.CloseSession(id2);

            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CloseSessions_SessionsExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid(); ;
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket2 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var ticket4 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval2 = 1900;
            var interval3 = 1100;
            var interval4 = 900;
            var ip1 = "127.0.0.1";
            var ip2 = "127.0.0.2";
            var ip3 = "127.0.0.3";
            var ip4 = "127.0.0.4";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate2 = DateTime.UtcNow.AddDays(-2);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var createDate4 = DateTime.UtcNow.AddDays(-5);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate2 = DateTime.UtcNow.AddDays(2);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var ExpiredDate4 = DateTime.UtcNow.AddDays(-3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate2 = DateTime.UtcNow.AddMinutes(-6);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);
            var lastAccessDate4 = DateTime.UtcNow.AddMinutes(-11);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id2, UserId = userId1, IP = ip2, Ticket = ticket2, UpdateExpireInc = interval2, CreatedDate = createDate2, LastAccessDate = lastAccessDate2, ExpiredDate = ExpiredDate2 },
                new Repositories.Entities.Session { Id = id3, UserId = userId2, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 },
                new Repositories.Entities.Session { Id = id4, UserId = userId1, IP = ip4, Ticket = ticket4, UpdateExpireInc = interval4, CreatedDate = createDate4, LastAccessDate = lastAccessDate4, ExpiredDate = ExpiredDate4 }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.CloseSessions(userId1);

            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result.Any(t => t.ExpiredDate < DateTime.UtcNow && t.Id == id1));
            Assert.IsTrue(result.Any(t => t.ExpiredDate < DateTime.UtcNow && t.Id == id2));
        }

        [TestMethod]
        public void CloseSessions_SessionsNotExisted_UseDbContextReturnCorrect()
        {
            AuthDBContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var ticket1 = Guid.NewGuid().ToString();
            var ticket3 = Guid.NewGuid().ToString();
            var interval1 = 900;
            var interval3 = 1100;
            var ip1 = "127.0.0.1";
            var ip3 = "127.0.0.3";
            var createDate1 = DateTime.UtcNow.AddDays(-1);
            var createDate3 = DateTime.UtcNow.AddDays(-3);
            var ExpiredDate1 = DateTime.UtcNow.AddDays(1);
            var ExpiredDate3 = DateTime.UtcNow.AddDays(3);
            var lastAccessDate1 = DateTime.UtcNow.AddMinutes(-3);
            var lastAccessDate3 = DateTime.UtcNow.AddMinutes(-9);

            var data = new List<Repositories.Entities.Session>
            {
                new Repositories.Entities.Session { Id = id1, UserId = userId1, IP = ip1, Ticket = ticket1, UpdateExpireInc = interval1, CreatedDate = createDate1, LastAccessDate = lastAccessDate1, ExpiredDate = ExpiredDate1 },
                new Repositories.Entities.Session { Id = id3, UserId = userId3, IP = ip3, Ticket = ticket3, UpdateExpireInc = interval3, CreatedDate = createDate3, LastAccessDate = lastAccessDate3, ExpiredDate = ExpiredDate3 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Repositories.Entities.Session>>();

            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Repositories.Entities.Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Repositories.Entities.Session>()));

            AuthDBContext.Setup(x => x.UserSession).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.Set<Repositories.Entities.Session>()).Returns(mockSet.Object);
            AuthDBContext.Setup(x => x.SaveChanges()).Returns(1);
            AuthDBContextFactory.Setup(x => x.CreateDBContext()).Returns(AuthDBContext.Object);

            var repository = new SessionRepository(AuthDBContextFactory.Object);

            var result = repository.CloseSessions(userId2);

            AuthDBContext.Verify(x => x.UserSession, Times.Exactly(1));
            AuthDBContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.IsFalse(result.Any());
        }
    }
}
