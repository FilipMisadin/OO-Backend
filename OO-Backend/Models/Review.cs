using OO_Backend.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class Review
    {
        [Column("ReviewId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Send User Id is required")]
        public int SendUserId { get; set; }
        [Required(ErrorMessage = "Receive User Id is required")]
        public int ReceiveUserId { get; set; }
        public string Body { get; set; }
        [Required(ErrorMessage = "Mark is required")]
        [Range(1, 5, ErrorMessage = "Mark must be between 1 and 5")]
        public int Mark { get; set; }
        public DateTime Date { get; set; }


        public ReviewResponse ToResponse(DatabaseContext database)
        {
            var sendUser = database.GetUser(this.SendUserId);
            var reviewResponse = new ReviewResponse
            {
                Id = this.Id,
                SendUser = sendUser.ToShortResponse(),
                ReceiveUserId = this.ReceiveUserId,
                Body = this.Body,
                Mark = this.Mark,
                Date = this.Date
            };

            return reviewResponse;
        }
    }
}
