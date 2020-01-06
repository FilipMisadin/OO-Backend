using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class DogModel
    {
        [Column("DogId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Owner Id is required")]
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Breed { get; set; }
    }
}
