using OO_Backend.Models;
using OO_Backend.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend
{
    public class Converters
    {
        public static UnauthorizedUserResponse UserModelToUnauthorizedUserResponse(UserModel user, DatabaseContext database)
        {
            var response = new UnauthorizedUserResponse 
            {
                Id = user.Id, 
                Username = user.Username,
                EmailAddress = user.EmailAddress, 
                ImageUrl = user.ImageUrl,
                FirstName = user.FirstName, 
                LastName = user.LastName, 
                BirthDate = user.BirthDate,
                Rating = user.Rating,
                Dogs = new List<DogModel>(), 
                Reviews = new List<ReviewResponse>()
            };

            var dogs = database.GetUserDogs(user.Id);
            response.Dogs.AddRange(dogs);

            var reviews = database.GetUserReviews(user.Id);
            response.Reviews.AddRange(reviews);

            return response;
        }
        /*
        public static OfferNotificationResponse OfferNotificationModelToOfferNotificationResponse(OfferNotificationModel notification, DatabaseContext database)
        {
            var requestData = database.GetNotificationRequestData(notification.Id);
            if (requestData == null)
            {
                requestData = new RequestNotificationModel();
            }
            var response = new OfferNotificationResponse
            {
                Id = notification.Id,
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
        }

        public static RespondResponse NotificationModelToRespondResponse(OfferNotificationModel notification, DatabaseContext database)
        {
            var requestData = database.GetNotificationRequestData(notification.Id);
            if (requestData == null)
            {
                requestData = new RequestNotificationModel();
            }
            var response = new RespondResponse
            {
                Id = notification.Id,
                SendUserId = notification.SendUserId,
                ReceivedUser = UserModelToShortUserResponse(database.GetUser(notification.ReceivedUserId)),
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

       

        /*public static RequestNotificationModel NotificationBodyModelToRequestNotificationModel(OfferNotificationBodyModel notification)
        {
            var response = new RequestNotificationModel
            {
                NotificationId = notification.Id,
                DogId = notification.DogId,
                Date = notification.Date,
                TimeFrom = notification.TimeFrom,
                TimeTo = notification.TimeTo,
                MeetAddress = notification.MeetAddress
            };

            return response;
        }

        public static OfferNotificationModel NotificationBodyModelToNotificationModel(OfferNotificationBodyModel notification)
        {
            var response = new OfferNotificationModel
            {
                Id = notification.Id,
                SendUserId = notification.SendUserId,
                ReceivedUserId = notification.ReceivedUserId,
                Status = notification.Status,
                RequestAdId = notification.AdId
            };

            return response;
        }*/

        public static OfferServicesAdModel OfferAdBodyToOfferAdModel(OfferAdBodyModel offer)
        {
            var response = new OfferServicesAdModel
            {
                Id = offer.Id,
                UserId = offer.UserId,
                Body = offer.Body,
                PostDate = offer.PostDate,
                DayAvailableFrom = offer.DayAvailableFrom,
                DayAvailableTo = offer.DayAvailableTo,
                HourAvailableFrom = offer.HourAvailableFrom,
                HourAvailableTo = offer.HourAvailableTo
            };

            return response;
        }

        public static OfferAdResponse OfferAdModelToOfferAdResponse(OfferServicesAdModel offerAd, DatabaseContext database)
        {
            OfferAdResponse responseOffer = new OfferAdResponse
            {
                Id = offerAd.Id,
                Body = offerAd.Body,
                PostDate = offerAd.PostDate,
                DayAvailableFrom = offerAd.DayAvailableFrom,
                DayAvailableTo = offerAd.DayAvailableTo,
                HourAvailableFrom = offerAd.HourAvailableFrom,
                HourAvailableTo = offerAd.HourAvailableTo
            };

            responseOffer.User = database.GetUser(offerAd.UserId).ToShortResponse();

            responseOffer.Neighborhoods = database.GetOfferNeighborhoods(offerAd.Id).Select(i => i.Name).ToList();

            return responseOffer;
        }

        public static RequestAdResponse RequestAdModelToRequestAdResponse(RequestServicesAdModel requestAd, DatabaseContext database)
        {
            RequestAdResponse response = new RequestAdResponse
            {
                Id = requestAd.Id,
                Body = requestAd.Body,
                Neighborhood = requestAd.Neighborhood,
                PostDate = requestAd.PostDate,
                MeetDate = requestAd.MeetDate,
                HourFrom = requestAd.HourFrom,
                HourTo = requestAd.HourTo
            };

            response.User = database.GetUser(requestAd.UserId).ToShortResponse();
            response.Dog = database.GetDog(requestAd.DogId);

            return response;
        }
    }
}
