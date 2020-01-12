using OO_Backend.Enums;
using OO_Backend.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class OfferServicesAdModel
    {
        [Column("OfferId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public int UserId { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public WeekDay DayAvailableFrom { get; set; }
        public WeekDay DayAvailableTo { get; set; }
        public int HourAvailableFrom { get; set; }
        public int HourAvailableTo { get; set; }


        public OfferAdResponse ToResponse(DatabaseContext database)
        {

            OfferAdResponse responseOffer = new OfferAdResponse
            {
                Id = this.Id,
                Body = this.Body,
                Date = this.PostDate,
                DayAvailableFrom = this.DayAvailableFrom,
                DayAvailableTo = this.DayAvailableTo,
                HourAvailableFrom = this.HourAvailableFrom,
                HourAvailableTo = this.HourAvailableTo
            };

            responseOffer.User = database.GetUser(this.UserId).ToShortResponse();

            responseOffer.Neighborhoods = database.GetOfferNeighborhoods(this.Id).Select(i => i.Name).ToList();

            return responseOffer;
        }
    }
}
