using Newtonsoft.Json;
using OO_Backend.Enums;
using OO_Backend.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class OfferNotification
    {
        [Column("OfferNotificationId")]
        public int Id { get; set; }
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceivedUserId { get; set; }
        public int RequestAdId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public NotificationStatus Status { get; set; }
        public DateTime PostDate { get; set; }

        [JsonIgnore]
        public User SendUser { get; set; }
        [JsonIgnore]
        public User ReceivedUser { get; set; }
        public RequestAd RequestAd { get; set; }

        public OfferNotificationResponse ToResponse(DatabaseContext database)
        {
            var response = new OfferNotificationResponse
            {
                Id = this.Id,
                SendUser = database.GetUser(this.SendUserId).ToShortResponse(),
                ReceivedUser = database.GetUser(this.ReceivedUserId).ToShortResponse(),
                RequestAd = database.GetRequestServicesAd(this.RequestAdId).ToResponse(database),
                Status = this.Status,
                PostDate = this.PostDate
            };

            return response;
        }
    }
}
