using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Column("UserId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float Rating { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
