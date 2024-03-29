﻿using Newtonsoft.Json;
using OO_Backend.Enums;
using OO_Backend.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OO_Backend.Models
{
    public class OfferAd
    {
        [Column("OfferId")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public WeekDay DayAvailableFrom { get; set; }
        public WeekDay DayAvailableTo { get; set; }
        public int HourAvailableFrom { get; set; }
        public int HourAvailableTo { get; set; }

        [JsonIgnore]
        public User User { set; get; }


        public OfferAdResponse ToResponse(DatabaseContext database)
        {

            var responseOffer = new OfferAdResponse
            {
                Id = this.Id,
                Body = this.Body,
                PostDate = this.PostDate,
                DayAvailableFrom = this.DayAvailableFrom,
                DayAvailableTo = this.DayAvailableTo,
                HourAvailableFrom = this.HourAvailableFrom,
                HourAvailableTo = this.HourAvailableTo,
                User = database.GetUser(this.UserId).ToShortResponse(),
                Neighborhoods = database.GetOfferNeighborhoods(this.Id).Select(i => i.Name).ToList()
            };



            return responseOffer;
        }
    }
}
