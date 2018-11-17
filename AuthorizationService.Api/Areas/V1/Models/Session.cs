using System;

namespace AuthorizationService.Api.Areas.V1.Models
{
    public class Session
    {
        public Session(AuthorizationService.Business.Models.Session session)
        {
            Id = session.Id;
            Ticket = session.Ticket;
            UserId = session.UserId;
            CreatedDate = session.CreatedDate;
            ExpiredDate = session.ExpiredDate;
            LastAccessDate = session.LastAccessDate;
            UpdateExpireInc = session.UpdateExpireInc;
            IP = session.IP;
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