using OO_Backend.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        /*public UserResponse ToResponse()
        {

        }*/

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
