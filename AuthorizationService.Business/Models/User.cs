using System;

namespace AuthorizationService.Business.Models
{
    public class User
    {
        public User()
        {
        }
        
        public User(Repositories.Entities.User entity)
        {
            Id = entity.Id;
            IsActive = entity.IsActive;
            Email = entity.Email;
            UserName = entity.UserName;
            Password = entity.Password;
            Salt = entity.Salt;
            CreatedDate = entity.CreatedDate;
            UpdatedDate = entity.UpdatedDate;
        }

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