using System;
using System.Linq;
using AuthorizationService.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Session = AuthorizationService.Api.Areas.V1.Models.Session;

namespace AuthorizationService.Api.Areas.V1.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Methods for viewing and managing user sessions.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class SessionsController : BaseController
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Initialisation.
        /// </summary>
        public SessionsController(ISessionService sessionService) : base()
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Returns list of active sessions.
        /// </summary>
        /// <returns>Status of request response (OK/500) and list of sessions in case of success execution.</returns>
        [HttpGet("active", Name = "GetActiveSessions")]
        public IActionResult GetActive()
        {
            try
            {
                var sessions = _sessionService.GetActiveSessions();

                return Ok(sessions.Select(t => new Session(t)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns session information by id.
        /// </summary>
        /// <param name="id">Session identifier</param>
        /// <returns>Status of request response (OK/404/500) and session details in case of success execute.</returns>
        [HttpGet("{id}", Name = "GetSession")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var session = _sessionService.GetSession(id);

                if (session == null)
                {
                    return NotFound();
                }

                return Ok(new Session(session));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for closing session by specified id.
        /// </summary>
        /// <param name="id">Session identifier</param>
        /// <returns>Status of request execution (ОК/404/500).</returns>
        // PUT api/sessions/{id}/close
        [HttpPut("{id}/close", Name = "CloseSession")]
        public IActionResult CloseSession(Guid id)
        {
            try
            {
                var result = _sessionService.CloseSession(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new Session(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for close all active user sessions.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Status of request execution (ОК/404/500).</returns>
        // PUT api/sessions/close?userid={userId}
        [HttpPut("close", Name = "CloseUserSessions")]
        public IActionResult CloseUserSessions(Guid userId)
        {
            try
            {
                var result = _sessionService.CloseSessions(userId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result.Select(t => new Session(t)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
