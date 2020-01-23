using OO_Backend.Models;
using System;

namespace OO_Backend.Responses
{
    public class RequestAdResponse
    {
        public int Id { get; set; }
        public ShortUserResponse User { get; set; }
        public string Neighborhood { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime MeetDate { get; set; }
        public int HourFrom { get; set; }
        public int HourTo { get; set; }
        public Dog Dog { get; set; }
    }
}
