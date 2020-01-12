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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DatabaseContext _database;

        public UserController(ILogger<UserController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpGet]
        [Route("users")]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            var users = _database.GetUsers();
            var response = new List<UnauthorizedUserResponse>();
            foreach (var user in users)
            {
                response.Add(_database.GetUser(user.Id).ToUnauthorizedResponse(_database));
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("user")]
        public IActionResult GetAuthorizedUser()
        {
            return Ok(_database.GetUser(User.Identity.Name).ToResponse(_database));            
        }

        [HttpGet]
        [Route("user/{id}")]
        [AllowAnonymous]
        public IActionResult GetUser(long id)
        {
            if (_database.UserExists(id))
            {
                return Ok(Converters.UserModelToUnauthorizedUserResponse(_database.GetUser(id), _database));
            } else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("user")]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] UserModel user)
        {
            if(_database.UsernameExists(user.Username))
            {
                return BadRequest(Constants.UsernameAlreadyExistError);
            }

            if(user.Password == "")
            {
                return BadRequest(Constants.PasswordRequiredError);
            }

            if(user.ImageUrl == "")
            {
                user.ImageUrl = Constants.DefaultImageUrl;
            }

            _logger.LogInformation("Add User for UserId: {UserId}", user.Id);
            _database.AddUser(user);
            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut]
        [Route("user/{id}")]
        public IActionResult PutUser(long id,[FromBody] UserModel user)
        {
            if (id != user.Id)
            {
                return BadRequest(Constants.WrongUserIdError);
            }

            if (!_database.UserExists(user.Id))
            {
                return BadRequest();
            }

            if (_database.UsernameExists(user.Username) && CheckUsername(user.Username))
            {
                return BadRequest(Constants.UsernameAlreadyExistError);
            }

            if (user.ImageUrl == "")
            {
                user.ImageUrl = Constants.DefaultImageUrl;
            }

            if (_database.IsOwner(user.Id, User))
            {
                try
                {
                    _database.UpdateUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete]
        [Route("user/{id}")]
        public ActionResult<UserModel> DeleteUser(long id)
        {
            var user = _database.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            if (_database.IsOwner(user.Id, User))
            {
                _database.RemoveUser(user);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }
        
        [HttpGet]
        [Route("user/{userId}/offerNotifications")]
        public List<OfferNotificationResponse> GetOfferNotifications(int userId)
        {
            var notifications = _database.GetUserOfferNotifications(userId);

            return notifications;
        }

        [HttpGet]
        [Route("user/{userId}/requestNotifications")]
        public List<RequestNotificationResponse> GetRequestNotifications(int userId)
        {
            var notifications = _database.GetUserRequestNotifications(userId);

            return notifications;
        }

        [HttpGet]
        [Route("user/{userId}/offerResponds")]
        public List<OfferNotificationResponse> GetOfferResponds(int userId)
        {
            var response = _database.GetUserOfferResponds(userId);

            return response;
        }

        [HttpGet]
        [Route("user/{userId}/requestResponds")]
        public List<RequestNotificationResponse> GetRequestResponds(int userId)
        {
            var response = _database.GetUserRequestResponds(userId);

            return response;
        }

        [HttpGet]
        [Route("user/{userId}/offerAds")]
        [AllowAnonymous]
        public List<OfferAdResponse> GetUserOfferAds(long userId)
        {
            var offers = _database.GetUserOfferServicesAds(userId);

            var response = new List<OfferAdResponse>();

            offers.ForEach(offer =>
            {
                response.Add(Converters.OfferAdModelToOfferAdResponse(offer, _database));
            });

            return response;
        }

        [HttpGet]
        [Route("user/{userId}/requestAds")]
        [AllowAnonymous]
        public List<RequestAdResponse> GetUserRequestAds(long userId)
        {
            var requests = _database.GetUserRequestServicesAds(userId);

            var response = new List<RequestAdResponse>();

            requests.ForEach(request =>
            {
                response.Add(Converters.RequestAdModelToRequestAdResponse(request, _database));
            });

            return response;
        }

        [HttpGet]
        [Route("user/{userId}/reviews")]
        [AllowAnonymous]
        public List<ReviewResponse> GetUserReviews(long userId)
        {
            return _database.GetUserReviews(userId);
        }

        [HttpGet]
        [Route("user/{userId}/dogs")]
        [AllowAnonymous]
        public List<DogModel> GetUserDogs(long userId)
        {
            return _database.GetUserDogs(userId);
        }

        private bool CheckUsername(string username)
        {
            return User.Identity.Name != username;
        }
    }
}
