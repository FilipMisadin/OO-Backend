using OO_Backend.Enums;
using OO_Backend.Models;
using System;

namespace OO_Backend.Responses
{
    public class RequestNotificationResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUser { get; set; }
        public ShortUserResponse ReceivedUser { get; set; }
        public OfferAdResponse OfferAd { get; set; }
        public NotificationStatus Status { get; set; }
        public DogModel Dog { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime MeetDate { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string MeetAddress { get; set; }
    }
}
