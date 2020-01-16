using OO_Backend.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public OfferServicesAdModel ToModel()
        {
            var response = new OfferServicesAdModel
            {
                Id = this.Id,
                UserId = this.UserId,
                Body = this.Body,
                PostDate = this.PostDate,
                DayAvailableFrom = this.DayAvailableFrom,
                DayAvailableTo = this.DayAvailableTo,
                HourAvailableFrom = this.HourAvailableFrom,
                HourAvailableTo = this.HourAvailableTo
            };

            return response;
        }
    }
}
