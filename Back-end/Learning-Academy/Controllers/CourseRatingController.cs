using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRatingController : ControllerBase
    {
        private readonly ICourseRatingRepository _ratingRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<User> _userManager;

        public CourseRatingController(
            ICourseRatingRepository ratingRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            UserManager<User> userManager)
        {
            _ratingRepository = ratingRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
        }


        private async Task<int> GetCurrentStudentIdAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var student = _studentRepository.GetByUserId(userId);
            if (student == null)
            {
                throw new InvalidOperationException("Student profile not found.");
            }

            return student.Id; // ده اللي هو int
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> AddRating(int courseId, [FromBody] CreateRatingDto createRatingDto)
        {
            int studentId = await GetCurrentStudentIdAsync();

            if (!await _ratingRepository.IsStudentEnrolledAsync(studentId, courseId))
            {
                return BadRequest("You must be enrolled in the course to rate it");
            }

            if (await _ratingRepository.HasStudentRatedCourseAsync(studentId, courseId))
            {
                return BadRequest("You have already rated this course");
            }

            var course = _courseRepository.GetByCourseId(courseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var rating = new CourseRating
            {
                CourseId = courseId,
                StudentId = studentId,
                RatingValue = createRatingDto.RatingValue,
                Comment = createRatingDto.Comment
            };

            var createdRating = await _ratingRepository.AddRatingAsync(rating);

            var averageRating = await _ratingRepository.GetAverageRatingAsync(courseId);
            var totalRatings = await _ratingRepository.GetTotalRatingsAsync(courseId);

            return Ok(new
            {
                Rating = new RatingDto
                {
                    Id = createdRating.Id,
                    RatingValue = createdRating.RatingValue,
                    Comment = createdRating.Comment,
                    CreatedAt = createdRating.CreatedAt,
                    StudentName = createdRating.Student.User.UserName
                },
                AverageRating = averageRating,
                TotalRatings = totalRatings
            });
        }

    }
}
