using OO_Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class RequestNotificationModel
    {
        public RequestNotificationModel() { }

        [Column("RequestNotificationId")]
        public int Id { get; set; }
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceivedUserId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public NotificationStatus Status { get; set; }
        public int DogId { get; set; }
        public DateTime Date { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string MeetAddress { get; set; }
        public int OfferAdId { get; set; }
    }
}
