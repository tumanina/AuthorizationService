using System;

namespace AuthorizationService.Business.Models
{
    public class Session
    {
        public Session()
        {

        }

        public Session(Repositories.Entities.Session entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Ticket = entity.Ticket;
            CreatedDate = entity.CreatedDate;
            ExpiredDate = entity.ExpiredDate;
            LastAccessDate = entity.LastAccessDate;
            UpdateExpireInc = entity.UpdateExpireInc;
            IP = entity.IP;
        }

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