using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthorizationService.Api.Areas.V1.Models;
using AuthorizationService.Business;

namespace AuthorizationService.Api.Areas.V1.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Methods for logging to system.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class LoginController : BaseController
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initialization.
        /// </summary>
        public LoginController(IAuthService authService, ILogger<LoginController> logger) : base(logger)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <returns>Execution status (ОК/401/500) and ticket or error info.</returns>
        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is empty or has invalid format.");
                }

                if (!IsIPv4(request.IP))
                {
                    return BadRequest("IP addres has invalid format.");
                }

                var result = _authService.Login(request.UserName, request.Password, request.IP);

                if (!result.IsAuth)
                {
                    return Unauthorized("Login or password are invalid or user is inactive.");
                }

                return Ok(result.Ticket);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
