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

            return student.Id; 
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
                    StudentName = createdRating.Student.UserName,
                    CourseName= createdRating.Course.CourseName
                   
                },
                AverageRating = averageRating,
                TotalRatings = totalRatings
            });
        }
        // GET: api/CourseRating/{courseId}
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetRatingsForCourse(int courseId)
        {
            var ratings = await _ratingRepository.GetRatingsForCourseAsync(courseId);

            if (ratings == null || !ratings.Any())
            {
                return NotFound("No ratings found for this course");
            }

            var ratingDtos = ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                RatingValue = r.RatingValue,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                StudentName = r.Student.UserName,
                
              
            });

            var averageRating = await _ratingRepository.GetAverageRatingAsync(courseId);
            var totalRatings = await _ratingRepository.GetTotalRatingsAsync(courseId);

            return Ok(new
            {
                Ratings = ratingDtos,
                AverageRating = averageRating,
                TotalRatings = totalRatings
            });
        }
        // GET: api/CourseRating/{courseId}/my-rating
        [HttpGet("{courseId}/my-rating")]
        public async Task<IActionResult> GetMyRatingForCourse(int courseId)
        {
            int studentId = await GetCurrentStudentIdAsync();

            var rating = await _ratingRepository.GetStudentRatingForCourseAsync(studentId, courseId);

            if (rating == null)
            {
                return NotFound("You haven't rated this course yet");
            }

            var ratingDto = new RatingDto
            {
                Id = rating.Id,
                RatingValue = rating.RatingValue,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                StudentName = rating.Student.UserName
            };

            return Ok(ratingDto);
        }

        // PUT: api/CourseRating/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] CreateRatingDto updateRatingDto)
        {
            int studentId = await GetCurrentStudentIdAsync();

            // 1. Get the existing rating just for validation
            var existingRating = await _ratingRepository.GetRatingByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound("Rating not found");
            }

            // 2. Verify ownership
            if (existingRating.StudentId != studentId)
            {
                return Forbid("You can only update your own ratings");
            }

            // 3. Update in repository (pass DTO and ID)
            var result = await _ratingRepository.UpdateRatingAsync(updateRatingDto, id);
            if (result == null)
            {
                return NotFound("Rating could not be updated");
            }

            // 4. Get updated statistics
            var averageRating = await _ratingRepository.GetAverageRatingAsync(result.CourseId);
            var totalRatings = await _ratingRepository.GetTotalRatingsAsync(result.CourseId);

            return Ok(new
            {
                Rating = new RatingDto
                {
                    Id = result.Id,
                    RatingValue = result.RatingValue,
                    Comment = result.Comment,
                    CreatedAt = result.CreatedAt,
                    StudentName = result.Student.UserName
                },
                AverageRating = averageRating,
                TotalRatings = totalRatings
            });
        }

        // DELETE: api/CourseRating/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            int studentId = await GetCurrentStudentIdAsync();

            var rating = await _ratingRepository.GetRatingByIdAsync(id);
            if (rating == null)
            {
                return NotFound("Rating not found");
            }

            if (rating.StudentId != studentId)
            {
                return Forbid("You can only delete your own ratings");
            }

            var courseId = rating.CourseId;
            await _ratingRepository.DeleteRatingAsync(id);

            var averageRating = await _ratingRepository.GetAverageRatingAsync(courseId);
            var totalRatings = await _ratingRepository.GetTotalRatingsAsync(courseId);

            return Ok(new
            {
                Message = "Rating deleted successfully",
                AverageRating = averageRating,
                TotalRatings = totalRatings
            });
        }
    }
}
