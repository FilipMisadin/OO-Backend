using System;
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
    public class RequestNotificationApiController : ControllerBase
    {
        private readonly ILogger<RequestNotificationApiController> _logger;
        private readonly DatabaseContext _database;

        public RequestNotificationApiController(ILogger<RequestNotificationApiController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpPost]
        [Route("requestNotification/{id}/accept")]
        public IActionResult AcceptRequestNotification(int id)
        {
            if (!_database.RequestNotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetRequestNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.AcceptRequestNotification(id);
            return Ok(notification);
        }

        [HttpPost]
        [Route("requestNotification/{id}/decline")]
        public IActionResult DeclineRequestNotification(int id)
        {
            if (!_database.RequestNotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetRequestNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.DeclineRequestNotification(id);
            return Ok(notification);
        }

        [HttpGet]
        [Route("requestNotification/{id}")]
        public IActionResult GetRequestNotification(int id)
        {
            if (_database.RequestNotificationExists(id))
            {
                var notification = _database.GetRequestNotification(id);

                if (_database.IsOwner(notification.ReceivedUserId, User))
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
        [Route("requestNotification")]
        public IActionResult AddRequestNotification([FromBody] RequestNotification notification)
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
            var notificationId = _database.AddRequestNotification(notification);
            notification.Id = notificationId;
            return Ok(notification);
        }

        // PUT: api/notification/5
        [HttpPut]
        [Route("requestNotification/{id}")]
        public IActionResult PutRequestNotification(int id, [FromBody] RequestNotification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            if (!_database.RequestNotificationExists(id))
            {
                return NotFound();
            }

            if (_database.IsOwner(notification.SendUserId, User))
            {
                try
                {
                    _database.UpdateRequestNotification(notification);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.RequestNotificationExists(id))
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

        // DELETE: api/notification/5
        [HttpDelete]
        [Route("requestNotification/{id}")]
        public ActionResult<RequestNotification> DeleteRequestNotification(int id)
        {
            if (_database.RequestNotificationExists(id))
            {
                return NotFound();
            }
            var notification = _database.GetRequestNotification(id);

            if (_database.IsOwner(notification.SendUserId, User))
            {
                _database.RemoveRequestNotification(notification);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
