using OO_Backend.Models;
using System;
using System.Collections.Generic;

namespace OO_Backend.Responses
{
    public class UnauthorizedUserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<DogModel> Dogs { get; set; }
        public List<ReviewResponse> Reviews { get; set; }
    }
}
