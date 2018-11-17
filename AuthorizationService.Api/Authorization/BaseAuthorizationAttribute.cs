using System;
using System.Collections.Generic;
using AuthorizationService.Api.Areas.V1.Controllers;
using AuthorizationService.Business;
using AuthorizationService.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Api.Authorization
{
    public abstract class BaseAuthorizationAttribute : TypeFilterAttribute
    {
        protected BaseAuthorizationAttribute(Type ticketAuthorizationFilter) : base(ticketAuthorizationFilter)
        {
        }
    }

    public class BaseAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;

        public static IEnumerable<Role> Roles;

        public BaseAuthorizationFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var message = string.Empty;
            var statusCode = 200;

            try
            {
                if (context.HttpContext.Items.TryGetValue("userId", out var userId))
                {
                    return;
                }

                var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authorization))
                {
                    statusCode = 401;
                    context.Result = new UnauthorizedResult();
                    message = "Authorize header is empty.";
                }
                else
                {
                    var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                        ? authorization.Substring("Bearer ".Length).Trim()
                        : authorization;

                    var registrationResult = _authService.Registrate(token, Roles);
                    message = registrationResult.Message;
                    statusCode = registrationResult.StatusCode;

                    if (statusCode == 200)
                    {
                        context.HttpContext.Items.Add("userId", registrationResult.UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                statusCode = 500;
                message = ex.InnerMessage();
            }
            finally
            {
                if (statusCode != 200)
                {
                    context.Result = statusCode == 401 ? new UnauthorizedResult() : new StatusCodeResult(statusCode);
                }
                
                context.HttpContext.Response.StatusCode = statusCode;
                if (!string.IsNullOrEmpty(message))
                {
                    context.HttpContext.Response.WriteAsync(message).ConfigureAwait(true);
                }
            }
        }
    }
}