using System;
using System.Collections.Generic;
using AuthorizationService.Repositories.Entities;

namespace AuthorizationService.Repositories
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetSessions(Guid userId);
        IEnumerable<Session> GetActiveSessions(Guid? userId);
        Session GetSession(Guid id);
        Session GetSessionByTicket(string ticket);
        Session CreateSession(Guid userId, int interval, string ip);
        bool ProlongSession(Guid id);
        Session CloseSession(Guid id);
        IEnumerable<Session> CloseSessions(Guid userId);
    }
}
