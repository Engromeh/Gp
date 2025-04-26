using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ICourseRepository _courseRepository;

        public QuestionController(IQuizRepository quizRepository, ICourseRepository courseRepository)
        {
            _quizRepository = quizRepository;

            _courseRepository = courseRepository;
        }

        // POST: api/quizzes/5/questions
        [HttpPost("{quizId}/questions")]
        public async Task<ActionResult> AddQuestion(int quizId, QuestionRequest request)
        {
            var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            var question = new Question
            {
                Text = request.Text,
                Points = request.Points,
                QuizId = quizId,
                Options = request.AnswerOptions.Select(ao => new Option
                {
                    text = ao.Text,
                    IsCorrect = ao.IsCorrect
                }).ToList()
            };

            await _quizRepository.AddQuestionAsync(question);

            return Ok (CreatedAtAction(nameof(GetQuestion), new { quizId, questionId = question.Id }, null));
        }

        // GET: api/quizzes/5/questions/10
        [HttpGet("{quizId}/questions/{questionId}")]
        public async Task<ActionResult<QuestionRequest>> GetQuestion(int quizId, int questionId)
        {
            var question = await _quizRepository.GetQuestionByIdAsync(questionId);
            if (question == null || question.QuizId != quizId)
            {
                return NotFound();
            }

            var response = new QuestionRequest
            {
                Text = question.Text,
                Points = question.Points,
                AnswerOptions = question.Options.Select(ao => new AnswerOptionRequest
                {
                    Text = ao.text,
                    IsCorrect = ao.IsCorrect
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int quizId, int questionId)
        {
            // First verify the question belongs to the specified quiz
            var question = await _quizRepository.GetQuestionByIdAsync(questionId);

            if (question == null || question.QuizId != quizId)
            {
                return NotFound("Question not found or doesn't belong to this quiz");
            }

            try
            {
                await _quizRepository.DeleteQuestionAsync(questionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the question");
            }
        }
        }
}
