using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        [Column("Date")]
        public DateTime PostDate { get; set; }
        //public DateTime RequestDate { get; set; }
        public int HourFrom { get; set; }
        public int HourTo { get; set; }
        [Required(ErrorMessage = "Dog Id is required")]
        public int DogId { get; set; }
    }
}
