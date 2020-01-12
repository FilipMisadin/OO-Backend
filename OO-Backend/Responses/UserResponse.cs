using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class UserResponse
    {
        public UserResponse(long id, string username, string emailAddress, string imageUrl,
            string firstName, string lastName, DateTime birthDate, double rating,
            List<DogModel> dogs, List<ReviewResponse> reviews, List<OfferNotificationResponse> offerNotifications,
            List<RequestNotificationResponse> requestNotifications,
            List<OfferNotificationResponse> offerResponds, List<RequestNotificationResponse> requestResponds)
        {
            Id = id;
            Username = username;
            EmailAddress = emailAddress;
            ImageUrl = imageUrl;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Dogs = dogs;
            Reviews = reviews;
            OfferNotifications = offerNotifications;
            RequestNotifications = requestNotifications;
            OfferResponds = offerResponds;
            RequestResponds = requestResponds;
            Rating = rating;
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<DogModel> Dogs { get; set; }
        public List<ReviewResponse> Reviews { get; set; }
        public List<OfferNotificationResponse> OfferNotifications { get; set; }
        public List<RequestNotificationResponse> RequestNotifications { get; set; }
        public List<OfferNotificationResponse> OfferResponds { get; set; }
        public List<RequestNotificationResponse> RequestResponds { get; set; }
    }
}
