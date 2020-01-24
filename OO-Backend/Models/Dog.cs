using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class Dog
    {
        [Column("DogId")]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Breed { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public User Owner { get; set; }
    }
}
