using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OO_Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class NotificationModel
    {
        [Column("NotificationId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Send User Id is required")]
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceivedUserId { get; set; }
        [JsonProperty("type")]
        [Required(ErrorMessage = "Type is required")]
        public NotificationType Type { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public NotificationStatus Status { get; set; }
        public int AdId { get; set; }
    }
}
