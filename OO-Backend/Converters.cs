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
        public static UserResponse UserModelToUserResponse(UserModel user, DatabaseContext database)
        {
            var response = new UserResponse(user.Id, user.Username, user.EmailAddress, user.ImageUrl,
                user.FirstName, user.LastName, user.BirthDate, user.Rating,
                new List<DogModel>(), new List<ReviewResponse>(), new List<NotificationResponse>(), new List<RespondResponse>());

            var dogs = database.GetUserDogs(user.Id);
            response.Dogs.AddRange(dogs);

            var reviews = database.GetUserReviews(user.Id);
            List<ReviewResponse> reviewResponses = new List<ReviewResponse>();

            reviews.ForEach(review =>
            {
                reviewResponses.Add(ReviewModelToReviewResponse(review, database));
            });

            response.Reviews.AddRange(reviewResponses);

            var notifications = database.GetUserNotifications(user.Id);
            List<NotificationResponse> notificationResponses = new List<NotificationResponse>();

            notifications.ForEach(notification =>
            {
                notificationResponses.Add(NotificationModelToNotificationResponse(notification, database));
            });
            response.Notifications.AddRange(notificationResponses);

            var responds = database.GetUserResponds(user.Id);
            List<RespondResponse> respondResponses = new List<RespondResponse>();

            responds.ForEach(respond =>
            {
                respondResponses.Add(NotificationModelToRespondResponse(respond, database));
            });
            response.Responds.AddRange(respondResponses);

            return response;
        }

        public static ShortUserResponse UserModelToShortUserResponse(UserModel user)
        {
            var response = new ShortUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                ImageUrl = user.ImageUrl,
                Rating = user.Rating
            };

            return response;
        }

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
            List<ReviewResponse> reviewResponses = new List<ReviewResponse>();

            reviews.ForEach(review =>
            {
                reviewResponses.Add(ReviewModelToReviewResponse(review, database));
            });

            response.Reviews.AddRange(reviewResponses);

            return response;
        }

        public static NotificationResponse NotificationModelToNotificationResponse(NotificationModel notification, DatabaseContext database)
        {
            var requestData = database.GetNotificationRequestData(notification.Id);
            if (requestData == null)
            {
                requestData = new RequestNotificationModel();
            }
            var response = new NotificationResponse
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
                MeetAddress = requestData.MeetAddress
            };

            return response;
        }

        public static RespondResponse NotificationModelToRespondResponse(NotificationModel notification, DatabaseContext database)
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
                Type = notification.Type,
                Status = notification.Status,
                Dog = database.GetDog(requestData.DogId),
                Date = requestData.Date,
                TimeFrom = requestData.TimeFrom,
                TimeTo = requestData.TimeTo,
                MeetAddress = requestData.MeetAddress
            };

            return response;
        }

        public static ReviewResponse ReviewModelToReviewResponse(ReviewModel review, DatabaseContext database)
        {
            var sendUser = database.GetUser(review.SendUserId);
            ReviewResponse reviewResponse = new ReviewResponse
            {
                Id = review.Id,
                SendUser = UserModelToShortUserResponse(sendUser),
                ReceiveUserId = review.ReceiveUserId,
                Body = review.Body,
                Mark = review.Mark
            };

            return reviewResponse;
        }

        public static RequestNotificationModel NotificationBodyModelToRequestNotificationModel(NotificationBodyModel notification)
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

        public static NotificationModel NotificationBodyModelToNotificationModel(NotificationBodyModel notification)
        {
            var response = new NotificationModel
            {
                Id = notification.Id,
                SendUserId = notification.SendUserId,
                ReceivedUserId = notification.ReceivedUserId,
                Type = notification.Type,
                Status = notification.Status
            };

            return response;
        }

        public static OfferServicesAdModel OfferAdBodyToOfferAdModel(OfferAdBodyModel offer)
        {
            var response = new OfferServicesAdModel
            {
                Id = offer.Id,
                UserId = offer.UserId,
                Body = offer.Body,
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
                DayAvailableFrom = offerAd.DayAvailableFrom,
                DayAvailableTo = offerAd.DayAvailableTo,
                HourAvailableFrom = offerAd.HourAvailableFrom,
                HourAvailableTo = offerAd.HourAvailableTo
            };

            responseOffer.User = UserModelToShortUserResponse(database.GetUser(offerAd.UserId));

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
                Date = requestAd.Date,
                HourFrom = requestAd.HourFrom,
                HourTo = requestAd.HourTo
            };

            response.User = UserModelToShortUserResponse(database.GetUser(requestAd.UserId));
            response.Dog = database.GetDog(requestAd.DogId);

            return response;
        }
    }
}
