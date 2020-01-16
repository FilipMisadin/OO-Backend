using System;

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
