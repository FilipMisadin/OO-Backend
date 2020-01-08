using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class ReviewModel
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
    }
}
