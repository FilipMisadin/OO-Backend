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
            List<DogModel> dogs, List<ReviewResponse> reviews/*, List<OfferNotificationResponse> notifications,
            List<RespondResponse> responds*/)
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
            //Notifications = notifications;
            Rating = rating;
            //Responds = responds;
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
        //public List<OfferNotificationResponse> Notifications { get; set; }
        //public List<RespondResponse> Responds { get; set; }
    }
}
