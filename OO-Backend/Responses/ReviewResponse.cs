using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Responses
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public ShortUserResponse SendUser { get; set; }
        public int ReceiveUserId { get; set; }
        public string Body { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
