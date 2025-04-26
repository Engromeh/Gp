using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class QuizSubmissionsController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;

        public QuizSubmissionsController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        // GET: api/quizsubmissions/5
        [HttpGet("{quizId}")]
        public async Task<ActionResult<QuizSubmissionResponse>> GetSubmission(int quizId)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var submission = await _quizRepository.GetStudentSubmissionAsync(quizId, studentId);

            if (submission == null)
            {
                return NotFound();
            }

            var response = new QuizSubmissionResponse
            {
                Id = submission.Id,
                QuizId = submission.QuizId,
                StudentId = submission.StudentId,
                SubmissionTime = submission.SubmissionTime,
                Score = submission.Score,
                Answers = submission.StudentAnswers.Select(a => new StudentAnswerResponse
                {
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    PointsEarned = a.PointsEarned
                }).ToList()
            };

            return Ok(response);
        }

        // POST: api/quizsubmissions
        [HttpPost]
        public async Task<ActionResult<QuizSubmissionResponse>> SubmitQuiz(QuizSubmissionRequest request)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Check if student already submitted
            var existingSubmission = await _quizRepository.GetStudentSubmissionAsync(request.QuizId, studentId);
            if (existingSubmission != null)
            {
                return BadRequest("You have already submitted this quiz.");
            }

            // Get the quiz with questions and correct answers
            var quiz = await _quizRepository.GetQuizWithQuestionsByIdAsync(request.QuizId);
            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            // Create new submission
            var submission = new QuizSubmission
            {
                QuizId = request.QuizId,
                StudentId = studentId,
                StudentAnswers = new List<StudentAnswer>(),
                Score = 0
            };

            // Calculate score
            foreach (var answer in request.Answers)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null) continue;

                var selectedOption = question.Options.FirstOrDefault(o => o.Id == answer.SelectedOptionId);
                var isCorrect = selectedOption?.IsCorrect ?? false;

                var studentAnswer = new StudentAnswer
                {
                    QuestionId = answer.QuestionId,
                    SelectedOptionId = answer.SelectedOptionId,
                    PointsEarned = isCorrect ? question.Points : 0
                };

                submission.Score += studentAnswer.PointsEarned;
                submission.StudentAnswers.Add(studentAnswer);
            }

            var createdSubmission = await _quizRepository.AddSubmissionAsync(submission);

            var response = new QuizSubmissionResponse
            {
                Id = createdSubmission.Id,
                QuizId = createdSubmission.QuizId,
                StudentId = createdSubmission.StudentId,
                SubmissionTime = createdSubmission.SubmissionTime,
                Score = createdSubmission.Score,
                Answers = createdSubmission.StudentAnswers.Select(a => new StudentAnswerResponse
                {
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    PointsEarned = a.PointsEarned
                }).ToList()
            };

            return CreatedAtAction(nameof(GetSubmission), new { quizId = response.QuizId }, response);
        }
    }
}
