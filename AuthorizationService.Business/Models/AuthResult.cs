using System;

namespace AuthorizationService.Business.Models
{
    public class AuthResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? UserId { get; set; }
    }
}
