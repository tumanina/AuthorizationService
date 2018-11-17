using System;

namespace AuthorizationService.Api.Areas.V1.Models
{
    public class User
    {
        public User()
        {
            
        }

        public User(AuthorizationService.Business.Models.User entity)
        {
            Id = entity.Id;
            IsActive = entity.IsActive;
            Email = entity.Email;
            UserName = entity.UserName;
            CreatedDate = entity.CreatedDate;
            UpdatedDate = entity.UpdatedDate;
        }

        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}