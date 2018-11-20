using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuthorizationService.Api.Areas.V1.Controllers
{
    public class BaseController: Controller
    {
        private readonly ILogger<BaseController> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        public BaseController()
        {

        }

        protected ObjectResult InternalServerError(Exception ex)
        {
            var message = ex.InnerMessage();
            _logger.LogError(ex, message);
            return StatusCode(500, message);
        }

        protected ObjectResult InternalServerError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, message);
        }

        protected ObjectResult Unauthorized(string message)
        {
            return StatusCode(401, message);
        }

        protected string GetCreatedUrl(string id)
        {
            return Request.Scheme + "://" + Request.Host + Request.Path + "/" + id;
        }

        protected static bool IsIPv4(string ipAddress)
        {
            return Regex.IsMatch(ipAddress, @"^\d{1,3}(\.\d{1,3}){3}$") &&
                   ipAddress.Split('.').SingleOrDefault(s => int.Parse(s) > 255) == null;
        }

        protected static bool IsEmailValid(string email)
        {
            String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                     + "@"
                                     + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, theEmailPattern);
        }
    }

    public static class Extensions
    {
        public static string InnerMessage(this Exception exception)
        {
            while (true)
            {
                if (exception.InnerException == null) return exception.Message;
                exception = exception.InnerException;
            }
        }
    }
}
