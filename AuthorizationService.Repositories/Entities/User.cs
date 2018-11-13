using System;

namespace AuthorizationService.Repositories.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}