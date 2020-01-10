using OO_Backend.Enums;
using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class RequestNotificationResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUserId { get; set; }
        public ShortUserResponse ReceivedUserId { get; set; }
        public NotificationStatus Status { get; set; }
        public DogModel DogId { get; set; }
        public DateTime Date { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string MeetAddress { get; set; }
        public int OfferAdId { get; set; }
    }
}
