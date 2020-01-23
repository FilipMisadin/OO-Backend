using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class Dog
    {
        [Column("DogId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Owner Id is required")]
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Breed { get; set; }
        public string ImageUrl { get; set; }
    }
}
