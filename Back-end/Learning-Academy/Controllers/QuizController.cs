using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Learning_Academy.DTO;
using Learning_Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Mono.TextTemplating;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ICourseRepository _courseRepository;

        public QuizController(IQuizRepository quizRepository, ICourseRepository courseRepository)
        {
            _quizRepository = quizRepository;

            _courseRepository = courseRepository;
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

        // POST: api/quizzes
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

