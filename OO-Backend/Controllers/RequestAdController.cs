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
    public class RequestAdController : ControllerBase
    {
        private readonly ILogger<RequestAdController> _logger;
        private readonly DatabaseContext _database;

        public RequestAdController(ILogger<RequestAdController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpGet]
        [Route("requestAds")]
        [AllowAnonymous]
        public List<RequestAdResponse> GetAllRequestAds()
        {
            return GetRequestServicesAds();
        }

        [HttpGet]
        [Route("requestAd/{id}")]
        [AllowAnonymous]
        public IActionResult GetRequestAd(long id)
        {
            if (_database.RequestServicesAdExists(id))
            {
                var requestAd = _database.GetRequestServicesAd(id);

                return Ok(requestAd);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("requestAd")]
        public IActionResult AddRequestServicesAd([FromBody] RequestServicesAdModel requestAd)
        {
            if (!_database.DogExists(requestAd.DogId))
            {
                return BadRequest(Constants.DogDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);

            var dog = _database.GetDog(requestAd.DogId);
            if(dog.OwnerId != ownerId)
            {
                return BadRequest(Constants.WrongDogOwnerError);
            }

            requestAd.UserId = ownerId;

            _logger.LogInformation("Add requestAd for requestAdId: {RequestAdId}", requestAd.Id);
            _database.AddRequestServicesAd(requestAd);
            return Ok(requestAd);
        }

        // PUT: api/RequestAd/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("requestAd/{id}")]
        public IActionResult PutRequestAd(long id, RequestServicesAdModel requestAd)
        {
            if (id != requestAd.Id)
            {
                return BadRequest();
            }

            if (_database.IsOwner(requestAd.UserId, User))
            {
                try
                {
                    _database.UpdateRequestServicesAd(requestAd);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.RequestServicesAdExists(id))
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

        // DELETE: api/RequestAd/5
        [HttpDelete]
        [Route("requestAd/{id}")]
        public ActionResult<RequestServicesAdModel> DeleteRequestAd(long id)
        {
            var requestAd = _database.GetRequestServicesAd(id);
            if (requestAd == null)
            {
                return NotFound();
            }

            if (_database.IsOwner(requestAd.UserId, User))
            {
                _database.RemoveRequestServicesAd(requestAd);
            }
            else
            {
                return Unauthorized();
            }


            return NoContent();
        }

        private List<RequestAdResponse> GetRequestServicesAds()
        {
            var ads = _database.GetRequestServicesAds();

            List<RequestAdResponse> requests = new List<RequestAdResponse>();

            ads.ForEach(ad =>
            {
                requests.Add(Converters.RequestAdModelToRequestAdResponse(ad, _database));
            });

            return requests;
        }
    }
}
