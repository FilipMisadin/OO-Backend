using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend.Models
{
    public class OfferToNeighborhood
    {
        public int NeighborhoodId { get; set; }
        public int OfferId { get; set; }
    }
}
