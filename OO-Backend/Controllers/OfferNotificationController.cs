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
    public class OfferNotificationController : ControllerBase
    {
        private readonly ILogger<OfferNotificationController> _logger;
        private readonly DatabaseContext _database;

        public OfferNotificationController(ILogger<OfferNotificationController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }
        
        [HttpPost]
        [Route("offerNotification/{id}/accept")]
        public IActionResult AcceptOfferNotification(int id)
        {
            if (!_database.OfferNotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetOfferNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.AcceptOfferNotification(id);
            return Ok(notification);
        }

        [HttpPost]
        [Route("offerNotification/{id}/decline")]
        public IActionResult DeclineOfferNotification(int id)
        {
            if (!_database.OfferNotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetOfferNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.DeclineOfferNotification(id);
            return Ok(notification);
        }

        [HttpGet]
        [Route("offerNotification/{id}")]
        public IActionResult GetOfferNotification(int id)
        {
            if (_database.OfferNotificationExists(id))
            {
                var notification = _database.GetOfferNotification(id);

                if(_database.IsOwner(notification.ReceivedUserId, User))
                {
                    return Ok(notification);
                }
                return Unauthorized();
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Route("offerNotification")]
        public IActionResult AddOfferNotification([FromBody] OfferNotificationModel notification)
        {
            if (!_database.UserExists(notification.ReceivedUserId))
            {
                return BadRequest(Constants.ReceiveUserIsNotValidError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);

            if (notification.ReceivedUserId == ownerId)
            {
                return BadRequest(Constants.UserCantNotifyHimselfError);
            }

            notification.SendUserId = ownerId;

            _logger.LogInformation("Add notification for notificationId: {notification}", notification.Id);
            var notificationId = _database.AddOfferNotification(notification);
            notification.Id = notificationId;
            return Ok(notification);
        }

        // PUT: api/notification/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("offerNotification/{id}")]
        public IActionResult PutNotification(int id,[FromBody] OfferNotificationModel notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            if (!_database.OfferNotificationExists(id))
            {
                return NotFound();
            }

            if (_database.IsOwner(notification.SendUserId, User))
            {
                try
                {
                    _database.UpdateOfferNotification(notification);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.OfferNotificationExists(id))
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

        // DELETE: api/notification/5
        [HttpDelete]
        [Route("offerNotification/{id}")]
        public ActionResult<OfferNotificationModel> DeleteNotification(int id)
        {
            if (_database.OfferNotificationExists(id))
            {
                return NotFound();
            }
            var notification = _database.GetOfferNotification(id);

            if (_database.IsOwner(notification.SendUserId, User))
            {
                _database.RemoveOfferNotification(notification);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
