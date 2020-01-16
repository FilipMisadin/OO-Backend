using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OO_Backend.Models;

namespace OO_Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class DogController : ControllerBase
    {
        private readonly ILogger<DogController> _logger;
        private readonly DatabaseContext _database;

        public DogController(ILogger<DogController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpGet]
        [Route("dogs")]
        [AllowAnonymous]
        public List<DogModel> GetAllDogs() => _database.GetDogs();
        
        [HttpGet]
        [Route("dog/{id}")]
        [AllowAnonymous]
        public IActionResult GetDog(int id)
        {
            if (_database.DogExists(id))
            {
                var dog = _database.GetDog(id);

                return Ok(dog);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("dog")]
        public IActionResult AddDog([FromBody] DogModel dog)
        {
            if(dog.Name == "")
            {
                return BadRequest(Constants.NameIsRequiredError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);

            if (!_database.UserExists(ownerId))
            {
                return BadRequest(Constants.DogOwnerDoesntExistError);
            }

            dog.OwnerId = ownerId;

            _logger.LogInformation("Add Dog for DogId: {DogId}", dog.Id);
            _database.AddDog(dog);
            return Ok(dog);
        }

        // PUT: api/Dogs/5
        [HttpPut]
        [Route("dog/{id}")]
        public IActionResult PutDog(int id, DogModel dog)
        {
            if (id != dog.Id)
            {
                return BadRequest();
            }

            if (_database.IsOwner(dog.OwnerId, User))
            {
                try
                {
                    _database.UpdateDog(dog);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.DogExists(id))
                    {
                        return NotFound();
                    }

                    throw;
                }
            }
            else
            {
                return Unauthorized();
            }


            return NoContent();
        }

        // DELETE: api/Dogs/5
        [HttpDelete]
        [Route("dog/{id}")]
        public ActionResult<DogModel> DeleteDog(long id)
        {
            var dog = _database.GetDog(id);
            if (dog == null)
            {
                return NotFound();
            }

            if (_database.IsOwner(dog.OwnerId, User))
            {
                _database.RemoveDog(dog);
            }
            else
            {
                return Unauthorized();
            }
            
            return NoContent();
        }
    }
}
