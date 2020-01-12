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
    public class RequestNotificationController : ControllerBase
    {
        private readonly ILogger<RequestNotificationController> _logger;
        private readonly DatabaseContext _database;

        public RequestNotificationController(ILogger<RequestNotificationController> logger, DatabaseContext context)
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
        public IActionResult AddRequestNotification([FromBody] RequestNotificationModel notification)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("requestNotification/{id}")]
        public IActionResult PutRequestNotification(int id, [FromBody] RequestNotificationModel notification)
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
        [Route("requestNotification/{id}")]
        public ActionResult<RequestNotificationModel> DeleteRequestNotification(int id)
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
