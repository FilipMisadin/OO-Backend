using OO_Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class OfferAdBodyModel
    {
        [Column("OfferId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public WeekDay DayAvailableFrom { get; set; }
        public WeekDay DayAvailableTo { get; set; }
        public List<string> Neighborhoods { get; set; }
        public int HourAvailableFrom { get; set; }
        public int HourAvailableTo { get; set; }
    }
}
