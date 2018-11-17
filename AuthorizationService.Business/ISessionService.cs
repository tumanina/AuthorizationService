using System;
using System.Collections.Generic;
using AuthorizationService.Business.Models;

namespace AuthorizationService.Business
{
    public interface ISessionService
    {
        IEnumerable<Session> GetActiveSessions();
        Session GetSession(Guid id);
        Session GetSessionByTicket(string ticket);
        bool ProlongSession(Guid id);
        Session CreateSession(Guid userId, int interval, string ip);
        Session CloseSession(Guid id);
        IEnumerable<Session> CloseSessions(Guid userId);
    }
}
