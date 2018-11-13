using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizationService.Repositories.Entities;
using AuthorizationService.Repositories.DAL;

namespace AuthorizationService.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IAuthDBContextFactory _factory;

        public SessionRepository(IAuthDBContextFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<Session> GetSessions(Guid userId)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.UserSession.Where(t => t.UserId == userId);
            }
        }

        public IEnumerable<Session> GetActiveSessions(Guid? userId = null)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.UserSession.Where(t => t.ExpiredDate >= DateTime.UtcNow && (!userId.HasValue || t.UserId == userId));
            }
        }

        public Session GetSession(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.UserSession.FirstOrDefault(t => t.Id == id);
            }
        }

        public Session GetSessionByTicket(string ticket)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.UserSession.FirstOrDefault(t => new Guid(t.Ticket) == new Guid(ticket));
            }
        }

        public Session CreateSession(Guid userId, int interval, string ip)
        {
            using (var context = _factory.CreateDBContext())
            {
                var currentDate = DateTime.UtcNow;

                var entity = new Session
                {
                    UserId = userId,
                    Ticket = Guid.NewGuid().ToString(),
                    CreatedDate = currentDate,
                    ExpiredDate = currentDate.AddSeconds(interval),
                    LastAccessDate = currentDate,
                    UpdateExpireInc = interval,
                    IP = ip
                };

                var sessions = context.Set<Session>();

                sessions.Add(entity);

                context.SaveChanges();

                return GetSession(entity.Id);
            }
        }

        public bool ProlongSession(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.UserSession.SingleOrDefault(b => b.Id == id);

                if (result == null)
                {
                    return false;
                }

                result.ExpiredDate = DateTime.UtcNow.AddSeconds(result.UpdateExpireInc);
                result.LastAccessDate = DateTime.UtcNow;
                context.SaveChanges();

                return true;
            }
        }

        public Session CloseSession(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.UserSession.SingleOrDefault(b => b.Id == id);

                if (result == null)
                {
                    return null;
                }

                result.ExpiredDate = DateTime.UtcNow.AddSeconds(-1);
                context.SaveChanges();

                return GetSession(id);
            }
        }

        public IEnumerable<Session> CloseSessions(Guid userId)
        {
            using (var context = _factory.CreateDBContext())
            {
                var sessions = context.UserSession.Where(b => b.UserId == userId && b.ExpiredDate >= DateTime.UtcNow);

                if (!sessions.Any())
                {
                    return new List<Session>();
                }

                var result = sessions.ToList();

                foreach (var session in sessions)
                {
                    session.ExpiredDate = DateTime.UtcNow.AddSeconds(-1);
                    result.FirstOrDefault(t => t.Id == session.Id).ExpiredDate = DateTime.UtcNow.AddSeconds(-1);
                }

                context.SaveChanges();

                return result;
            }
        }
    }
}
