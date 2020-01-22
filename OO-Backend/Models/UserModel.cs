using OO_Backend.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Column("UserId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float Rating { get; set; }
        public DateTime BirthDate { get; set; }


        public ShortUserResponse ToShortResponse()
        {
            var response = new ShortUserResponse
            {
                Id = this.Id,
                Username = this.Username,
                ImageUrl = this.ImageUrl,
                Rating = this.Rating
            };

            return response;
        }

        public UserResponse ToResponse(DatabaseContext database)
        {
            var response = new UserResponse(this.Id, this.Username, this.EmailAddress, this.ImageUrl,
               this.FirstName, this.LastName, this.BirthDate, this.Rating,
               new List<DogModel>(), new List<ReviewResponse>(), new List<OfferNotificationResponse>()
               , new List<RequestNotificationResponse>(), new List<OfferNotificationResponse>(), new List<RequestNotificationResponse>());

            var dogs = database.GetUserDogs(this.Id);
            response.Dogs.AddRange(dogs);

            var reviews = database.GetUserReviews(this.Id);
            response.Reviews.AddRange(reviews);

            var offerNotifications = database.GetUserOfferNotifications(this.Id);
            response.OfferNotifications.AddRange(offerNotifications);

            var requestNotifications = database.GetUserRequestNotifications(this.Id);
            response.RequestNotifications.AddRange(requestNotifications);

            var offerResponds = database.GetUserOfferResponds(this.Id);
            response.OfferResponds.AddRange(offerResponds);

            var requestResponds = database.GetUserRequestResponds(this.Id);
            response.RequestResponds.AddRange(requestResponds);

            return response;
        }

        public UnauthorizedUserResponse ToUnauthorizedResponse(DatabaseContext database)
        {
            var response = new UnauthorizedUserResponse
            {
                Id = this.Id,
                Username = this.Username,
                EmailAddress = this.EmailAddress,
                ImageUrl = this.ImageUrl,
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthDate = this.BirthDate,
                Rating = this.Rating,
                Dogs = new List<DogModel>(),
                Reviews = new List<ReviewResponse>()
            };

            var dogs = database.GetUserDogs(this.Id);
            response.Dogs.AddRange(dogs);

            var reviews = database.GetUserReviews(this.Id);
            response.Reviews.AddRange(reviews);

            return response;
        }
    }
}
