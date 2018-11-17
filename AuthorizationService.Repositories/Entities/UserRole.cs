using System;

namespace AuthorizationService.Repositories.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
    }
}
