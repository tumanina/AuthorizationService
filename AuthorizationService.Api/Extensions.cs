using System;
using System.Linq;

namespace AuthorizationService.Api.Business
{
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
