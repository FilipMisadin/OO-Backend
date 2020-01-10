using OO_Backend.Enums;
using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class OfferAdResponse
    {
        public int Id { get; set; }
        public ShortUserResponse User { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public WeekDay DayAvailableFrom { get; set; }
        public WeekDay DayAvailableTo { get; set; }
        public int HourAvailableFrom { get; set; }
        public int HourAvailableTo { get; set; }
        public List<string> Neighborhoods { get; set; }
    }
}
