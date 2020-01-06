using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class ShortUserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
    }
}
