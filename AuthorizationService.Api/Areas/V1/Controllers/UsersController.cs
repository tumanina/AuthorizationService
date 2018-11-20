using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Api.Areas.V1.Models;
using User = AuthorizationService.Api.Areas.V1.Models.User;
using AuthorizationService.Business;
using System.Linq;
using AuthorizationService.Api.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Api.Areas.V1.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Methods for viewing and managing users.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ManageUsers]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initialization.
        /// </summary>
        public UsersController(IUserService userService, ILogger<UsersController> logger) : base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns user in system by name/email.
        /// </summary>
        /// <param name="name">User name</param>
        /// <param name="email">User email</param>
        /// <returns>Execution status (ОК/500) and list of users/error info.</returns>
        [HttpGet]
        public IActionResult Get(string name = null, string email = null)
        {
            try
            {
                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }

                var user = !string.IsNullOrEmpty(name) ? _userService.GetUserByName(name) : (!string.IsNullOrEmpty(email) ?_userService.GetUserByEmail(email) : null);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new User(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns user details by Id.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Execution status (ОК/500) and user details/error info.</returns>
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var user = _userService.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new User(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns user roles.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Execution status (ОК/500) and list of roles/error info.</returns>
        [HttpGet("{id}/roles", Name = "GetUserRoles")]
        public IActionResult GetRolesById(Guid id)
        {
            try
            {
                var userRoles = _userService.GetUserRoles(id);

                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns user sessions.
        /// </summary>
        /// /// <param name="id">User identifier</param>
        /// <param name="onlyActive">Show only active sessions</param>
        /// <returns>Execution status (ОК/500) and list of sessions/error info.</returns>
        [HttpGet("{id}/sessions", Name = "GetUserSessions")]
        public IActionResult GetSessionsById(Guid id, bool onlyActive = true)
        {
            try
            {
                var userSessions = _userService.GetSessions(id, onlyActive);
                return Ok(userSessions.Select(t => new Session(t)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns user own sessions.
        /// </summary>
        /// <param name="onlyActive">Show only active sessions</param>
        /// <returns>Execution status (ОК/500) and list of sessions/error info.</returns>
        [HttpGet("own/sessions", Name = "GetUserOwnSessions")]
        [OwnerAccess]
        public IActionResult GetOwnSessions(bool onlyActive = true)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("userId", out var userId))
                {
                    var userSessions = _userService.GetSessions((Guid)userId, onlyActive);
                    return Ok(userSessions.Select(t => new Session(t)));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Method for creating new user.
        /// </summary>
        /// <param name="request">User details (email, username, password)</param>
        /// <returns>Execution status (ОК/500) and created user details.</returns>
        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]CreateUserRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is empty or has invalid format");
                }

                if (string.IsNullOrEmpty(request.UserName) || request.UserName.Length > 32)
                {
                    return BadRequest("User name is empty or has length more than 32.");
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Password is empty.");
                }

                if (string.IsNullOrEmpty(request.Email) || !IsEmailValid(request.Email))
                {
                    return BadRequest("Email is empty or has invalid format.");
                }

                var result = _userService.CreateUser(request.Email, request.UserName, request.Password);

                if (result == null)
                {
                    return StatusCode(500, "User didn't create.");
                }

                return Created(GetCreatedUrl(result.Id.ToString()), new User(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for update existing user.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="request">User details</param>
        /// <returns>Execution status (ОК/500) and updated user details.</returns>
        // PUT api/users/{id}
        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult Put(Guid id, [FromBody]UpdateUserRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request empty or has invalid format");
                }

                if (string.IsNullOrEmpty(request.UserName) || request.UserName.Length > 32)
                {
                    return BadRequest("User name is empty or has length more than 32.");
                }

                if (string.IsNullOrEmpty(request.Email) || !IsEmailValid(request.Email))
                {
                    return BadRequest("Email is empty or has invalid format.");
                }

                var result = _userService.UpdateUser(id, request.Email, request.UserName);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new User(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for changing user password.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="newPassword">new password</param>
        /// <returns>Execution status (ОК/500) and updated user details.</returns>
        // PUT api/users/{id}/password
        [HttpPut("{id}/password", Name = "ChangePassword")]
        public IActionResult Put(Guid id, [FromBody]string newPassword)
        {
            try
            {
                var result = _userService.ChangePassword(id, newPassword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for blocking user by specified id.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Execution status (ОК/500).</returns>
        // PUT api/users/{id}/block
        [HttpPut("{id}/block", Name = "BlockUser")]
        public IActionResult Block(Guid id)
        {
            try
            {
                var result = _userService.UpdateActive(id, false);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new User(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for unblocking user by specified id.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Execution status (ОК/500).</returns>
        // PUT api/users/{id}/unblock
        [HttpPut("{id}/unblock", Name = "UnblockUser")]
        public IActionResult UnBlock(Guid id)
        {
            try
            {
                var result = _userService.UpdateActive(id, true);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new User(result));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Method for setting roles for user.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <param name="roleIds">List of roles ids</param>
        /// <returns>Execution status (ОК/500).</returns>
        // PUT api/users/{id}/roles
        [HttpPut("{id}/roles", Name = "SetUserRoles")]
        public IActionResult SetRoles(Guid id, [FromBody] IEnumerable<int> roleIds)
        {
            try
            {
                _userService.UpdateRoles(id, roleIds);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
