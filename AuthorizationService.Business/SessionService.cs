using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizationService.Business.Models;
using AuthorizationService.Repositories;

namespace AuthorizationService.Business
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public IEnumerable<Session> GetActiveSessions()
        {
            var sessions = _sessionRepository.GetActiveSessions(null);

            return sessions.Select(t => new Session(t));
        }

        public Session GetSession(Guid id)
        {
            var node = _sessionRepository.GetSession(id);

            return node == null ? null : new Session(node);
        }

        public bool ProlongSession(Guid id)
        {
            return _sessionRepository.ProlongSession(id);
        }

        public Session CreateSession(Guid userId, int interval, string ip)
        {
            var createdSession = _sessionRepository.CreateSession(userId, interval, ip);

            return createdSession == null ? null : new Session(createdSession);
        }

        public Session CloseSession(Guid id)
        {
            var closedSession = _sessionRepository.CloseSession(id);

            return closedSession == null ? null : new Session(closedSession);
        }

        public IEnumerable<Session> CloseSessions(Guid userId)
        {
            var closedSessions = _sessionRepository.CloseSessions(userId);

            return closedSessions.Any() ? closedSessions.Select(t => new Session(t)) : new List<Session>();
        }
    }
}
