using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OO_Backend.Enums;
using OO_Backend.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class OfferNotificationModel
    {
        [Column("OfferNotificationId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Send User Id is required")]
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceivedUserId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public NotificationStatus Status { get; set; }
        public int RequestAdId { get; set; }


        /*public OfferNotificationResponse ToResponse()
        {
            var response = new OfferNotificationResponse
            {
                Id = this.Id,
                SendUser = UserModelToShortUserResponse(database.GetUser(notification.SendUserId)),
                ReceivedUserId = notification.ReceivedUserId,
                Type = notification.Type,
                Status = notification.Status,
                Dog = database.GetDog(requestData.DogId),
                Date = requestData.Date,
                TimeFrom = requestData.TimeFrom,
                TimeTo = requestData.TimeTo,
                MeetAddress = requestData.MeetAddress,
                AdNumber = notification.Type + "#" + notification.AdId.ToString().PadLeft(4, '0')
            };

            return response;
        }*/
    }
}
