using OO_Backend.Enums;
using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUser { get; set; }
        public int ReceivedUserId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public DogModel Dog { get; set; }
        public DateTime Date { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string MeetAddress { get; set; }
    }
}
