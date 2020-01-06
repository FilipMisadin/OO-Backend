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
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly DatabaseContext _database;

        public NotificationController(ILogger<NotificationController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpPost]
        [Route("notification/{id}/accept")]
        public IActionResult AcceptNotification(int id)
        {
            if (!_database.NotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.AcceptNotification(id);
            return Ok(notification);
        }

        [HttpPost]
        [Route("notification/{id}/decline")]
        public IActionResult DeclineNotification(int id)
        {
            if (!_database.NotificationExists(id))
            {
                return BadRequest(Constants.NotificationDoesntExistError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);
            var notification = _database.GetNotification(id);

            if (notification.ReceivedUserId != ownerId)
            {
                return Unauthorized();
            }

            _database.DeclineNotification(id);
            return Ok(notification);
        }

        [HttpGet]
        [Route("notification/{id}")]
        public IActionResult GetNotification(int id)
        {
            if (_database.NotificationExists(id))
            {
                var notification = _database.GetNotification(id);

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
        [Route("notification")]
        public IActionResult AddNotification([FromBody] NotificationBodyModel notification)
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
            var notificationId = _database.AddNotification(Converters.NotificationBodyModelToNotificationModel(notification));
            if(notification.DogId != 0)
            {
                _database.AddRequestNotification(Converters.NotificationBodyModelToRequestNotificationModel(notification), notificationId);
            }
            notification.Id = notificationId;
            return Ok(notification);
        }

        // PUT: api/notification/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("notification/{id}")]
        public IActionResult PutNotification(int id,[FromBody] NotificationBodyModel notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            if (!_database.NotificationExists(id))
            {
                return NotFound();
            }

            if (_database.IsOwner(notification.SendUserId, User))
            {
                try
                {
                    _database.UpdateNotification(Converters.NotificationBodyModelToNotificationModel(notification));
                    _database.UpdateRequestNotification(Converters.NotificationBodyModelToRequestNotificationModel(notification));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.NotificationExists(id))
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
        [Route("notification/{id}")]
        public ActionResult<NotificationModel> DeleteNotification(int id)
        {
            if (_database.NotificationExists(id))
            {
                return NotFound();
            }
            var notification = _database.GetNotification(id);

            if (_database.IsOwner(notification.SendUserId, User))
            {
                _database.RemoveNotification(notification);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
