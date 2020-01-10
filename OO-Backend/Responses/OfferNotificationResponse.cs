using OO_Backend.Enums;
using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class OfferNotificationResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUserId { get; set; }
        public ShortUserResponse ReceivedUserId { get; set; }
        public NotificationStatus Status { get; set; }
        public int RequestAdId { get; set; }
    }
}
