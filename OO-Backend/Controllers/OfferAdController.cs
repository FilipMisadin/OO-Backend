using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OO_Backend.Models;
using OO_Backend.Responses;

namespace OO_Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class OfferAdController : ControllerBase
    {
        private readonly ILogger<OfferAdController> _logger;
        private readonly DatabaseContext _database;

        public OfferAdController(ILogger<OfferAdController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpGet]
        [Route("offerAds")]
        [AllowAnonymous]
        public List<OfferAdResponse> GetAllOfferAds() 
        {
            return GetOfferServicesAds();
        }

        [HttpGet]
        [Route("offerAd/{id}")]
        [AllowAnonymous]
        public IActionResult GetOfferAd(long id)
        {
            if (_database.OfferServicesAdExists(id))
            {
                var offerAd = _database.GetOfferServicesAd(id);

                return Ok(offerAd);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("offerAd")]
        public IActionResult AddOfferServicesAd([FromBody] OfferAdBodyModel offerAd)
        {
            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);

            offerAd.UserId = ownerId;

            _logger.LogInformation("Add OfferAd for OfferAdId: {OfferAdId}", offerAd.Id);
            var offer = _database.AddOfferServicesAd(offerAd);
            return Ok(offer);
        }

        // PUT: api/offerAd/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("offerAd/{id}")]
        public IActionResult PutOfferAd(long id, OfferAdBodyModel offerAd)
        {
            if (id != offerAd.Id)
            {
                return BadRequest();
            }

            if (_database.IsOwner(offerAd.UserId, User))
            {
                try
                {
                    _database.UpdateOfferServicesAd(offerAd);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.OfferServicesAdExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }

        // DELETE: api/offerAd/5
        [HttpDelete]
        [Route("offerAd/{id}")]
        public ActionResult<OfferServicesAdModel> DeleteOfferAd(long id)
        {
            var offerAd = _database.GetOfferServicesAd(id);
            if (offerAd == null)
            {
                return NotFound();
            }

            if (_database.IsOwner(offerAd.UserId, User))
            {
                _database.RemoveOfferServicesAd(offerAd);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }

        private List<OfferAdResponse> GetOfferServicesAds()
        {
            var ads = _database.GetOfferServicesAds();

            List<OfferAdResponse> offers = new List<OfferAdResponse>();

            ads.ForEach(ad =>
            {
                offers.Add(Converters.OfferAdModelToOfferAdResponse(ad, _database));
            });

            return offers;
        }
    }
}
