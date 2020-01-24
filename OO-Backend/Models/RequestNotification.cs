using Newtonsoft.Json;
using OO_Backend.Enums;
using OO_Backend.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class RequestNotification
    {
        [Column("RequestNotificationId")]
        public int Id { get; set; }
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceivedUserId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public int OfferAdId { get; set; }
        public NotificationStatus Status { get; set; }
        public int DogId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime MeetDate { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public string MeetAddress { get; set; }


        [JsonIgnore]
        public User SendUser { set; get; }
        [JsonIgnore]
        public User ReceivedUser { set; get; }
        public OfferAd OfferAd { set; get; }
        public Dog Dog { set; get; }

        public RequestNotificationResponse ToResponse(DatabaseContext database)
        {
            var response = new RequestNotificationResponse
            {
                Id = this.Id,
                SendUser = database.GetUser(this.SendUserId).ToShortResponse(),
                ReceivedUser = database.GetUser(this.ReceivedUserId).ToShortResponse(),
                OfferAd = database.GetOfferServicesAd(this.OfferAdId).ToResponse(database),
                Status = this.Status,
                Dog = database.GetDog(this.DogId),
                PostDate = this.PostDate,
                MeetDate = this.MeetDate,
                TimeFrom = this.TimeFrom,
                TimeTo = this.TimeTo,
                MeetAddress = this.MeetAddress
            };

            return response;
        }
    }
}
