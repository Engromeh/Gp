using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Academy.DTO;
using Learning_Academy.Models.QuizModels;
using Microsoft.AspNetCore.Authorization;
using Mono.TextTemplating;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Learning_Academy.Models;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly LearningAcademyContext _context;

        public QuizController(IQuizRepository quizRepository, ICourseRepository courseRepository,LearningAcademyContext learningAcademyContext )
        {
            _quizRepository = quizRepository;

            _courseRepository = courseRepository;
            _context = learningAcademyContext;
        }

        // GET: api/quizzes/course/5
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<List<QuizResponse>>> GetQuizzesByCourse(int courseId)
        {
            var quizzes = await _quizRepository.GetQuizzesByCourseIdAsync(courseId);
            var response = quizzes.Select(q => new QuizResponse
            {
                Id = q.Id,
                Title = q.Title,
                CourseId = q.CourseId,
                DueDate = q.DueDate,
                TimeLimitMinutes = q.TimeLimitMinutes
            }).ToList();

            return Ok(response);
        }

        // GET: api/quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizResponse>> GetQuiz(int id)
        {
            var quiz = await _quizRepository.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var response = new QuizResponse
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                DueDate = quiz.DueDate,
                TimeLimitMinutes = quiz.TimeLimitMinutes
            };

            return Ok(response);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("my/quizzes")]
        public async Task<ActionResult<List<QuizResponse>>> GetMyQuizzes()
        {
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (instructor == null)
                return Unauthorized("Instructor not found.");

            var instructorId = instructor.Id;

            
            var courseIds = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => c.Id)
                .ToListAsync();

            if (!courseIds.Any())
                return Ok(new List<QuizResponse>());

            
            var quizzes = await _context.Quizzes
                .Where(q => courseIds.Contains(q.CourseId))
                .Select(q => new QuizResponse
                {
                    Id = q.Id,
                    Title = q.Title,
                    CourseId = q.CourseId,
                    DueDate = q.DueDate,
                    TimeLimitMinutes = q.TimeLimitMinutes
                })
                .ToListAsync();

            return Ok(quizzes);
        }





        // POST: api/quizzes
        [Authorize(Roles ="instructor")]
        [HttpPost]
        public async Task<ActionResult<QuizResponse>> CreateQuiz(QuizCreateRequest request)
        {
            var quiz = new Quiz
            {
                Title = request.Title,
                CourseId = request.CourseId,
                DueDate = request.DueDate,
                TimeLimitMinutes = request.TimeLimitMinutes
            };

            var createdQuiz = await _quizRepository.AddQuizAsync(quiz);

            var response = new QuizResponse
            {
                Id = createdQuiz.Id,
                Title = createdQuiz.Title,
                CourseId = createdQuiz.CourseId,
                DueDate = createdQuiz.DueDate,
                TimeLimitMinutes = createdQuiz.TimeLimitMinutes
            };

            return CreatedAtAction(nameof(GetQuiz), new { id = response.Id }, response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _quizRepository.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            await _quizRepository.DeleteQuizAsync(id);
            return NoContent();
        }

       
        
    }

}

