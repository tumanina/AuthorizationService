using System;

namespace AuthorizationService.Repositories.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Ticket { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime LastAccessDate { get; set; }
        public int UpdateExpireInc { get; set; }
        public string IP { get; set; }
    }
}