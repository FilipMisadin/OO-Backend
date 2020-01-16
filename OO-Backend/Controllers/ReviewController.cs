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
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly DatabaseContext _database;

        public ReviewController(ILogger<ReviewController> logger, DatabaseContext context)
        {
            _logger = logger;
            _database = context;
        }

        [HttpGet]
        [Route("review/{id}")]
        [AllowAnonymous]
        public IActionResult GetReview(long id)
        {
            if (!_database.ReviewExists(id)) return NotFound();
            var review = _database.GetReview(id);

            return Ok(review);
        }

        [HttpPost]
        [Route("review")]
        public IActionResult AddReview([FromBody] ReviewModel review)
        {
            if(!_database.UserExists(review.ReceiveUserId))
            {
                return BadRequest(Constants.ReceiveUserIsNotValidError);
            }

            var ownerId = Convert.ToInt32(_database.GetUser(User.Identity.Name).Id);

            if (review.ReceiveUserId == ownerId)
            {
                return BadRequest(Constants.UserCantRateHimselfError);
            }

            review.SendUserId = ownerId;

            _logger.LogInformation("Add Review for ReviewId: {ReviewId}", review.Id);
            _database.AddReview(review);
            UpdateUserRating(review.ReceiveUserId);
            return Ok(review);
        }

        // PUT: api/Review/5
        [HttpPut]
        [Route("review/{id}")]
        public IActionResult PutReview(long id, ReviewModel review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            if (_database.IsOwner(review.SendUserId, User))
            {
                try
                {
                    _database.UpdateReview(review);
                    UpdateUserRating(review.ReceiveUserId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_database.ReviewExists(id))
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

        // DELETE: api/Review/5
        [HttpDelete]
        [Route("review/{id}")]
        public ActionResult<ReviewModel> DeleteReview(long id)
        {
            var review = _database.GetReview(id);
            if (review == null)
            {
                return NotFound();
            }

            if (_database.IsOwner(review.SendUserId, User))
            {
                _database.RemoveReview(review);
                UpdateUserRating(review.ReceiveUserId);
            }
            else
            {
                return Unauthorized();
            }

            return NoContent();
        }

        private void UpdateUserRating(long userId)
        {
            var reviews = _database.GetUserReviews(userId);

            if(reviews.Count == 0)
            {
                return;
            }

            var sum = 0.0f;
            reviews.ForEach(review =>
            {
                sum += review.Mark;
            });

            _database.UpdateUserRating(userId, sum / reviews.Count);
        }
    }
}
