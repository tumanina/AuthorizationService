namespace AuthorizationService.Api.Areas.V1.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
    }
}
