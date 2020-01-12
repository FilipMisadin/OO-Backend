using OO_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class RequestAdResponse
    {
        public int Id { get; set; }
        public ShortUserResponse User { get; set; }
        public string Neighborhood { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public int HourFrom { get; set; }
        public int HourTo { get; set; }
        public DogModel Dog { get; set; }
    }
}
