using System.ComponentModel.DataAnnotations.Schema;

namespace OO_Backend.Models
{
    public class Neighborhood
    {
        [Column("NeighborhoodId")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
