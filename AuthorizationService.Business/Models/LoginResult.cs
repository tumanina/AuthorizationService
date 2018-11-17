namespace AuthorizationService.Business.Models
{
    public class LoginResult
    {
        public bool IsAuth { get; set; }
        public string Ticket { get; set; }
    }
}
