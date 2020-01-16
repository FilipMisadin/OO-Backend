using OO_Backend.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class RequestServicesAdModel
    {
        [Column("RequestId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
        public string Neighborhood { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime MeetDate { get; set; }
        public int HourFrom { get; set; }
        public int HourTo { get; set; }
        [Required(ErrorMessage = "Dog Id is required")]
        public int DogId { get; set; }


        public RequestAdResponse ToResponse(DatabaseContext database)
        {
            RequestAdResponse response = new RequestAdResponse
            {
                Id = this.Id,
                Body = this.Body,
                Neighborhood = this.Neighborhood,
                PostDate = this.PostDate,
                MeetDate = this.MeetDate,
                HourFrom = this.HourFrom,
                HourTo = this.HourTo,
                User = database.GetUser(this.UserId).ToShortResponse(),
                Dog = database.GetDog(this.DogId)
            };


            return response;
        }
    }
}
