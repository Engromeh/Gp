using Learning_Academy.DTO;
using Learning_Academy.Models.QuizModels;
using Learning_Academy.Repositories.Classes;
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
        private readonly IStudentRepository _studentRepository;

        public QuizSubmissionsController(IQuizRepository quizRepository,IStudentRepository studentRepository)
        {
            _quizRepository = quizRepository;
            _studentRepository = studentRepository;
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

        // GET: api/quizsubmissions/5
        [HttpGet("{quizId}")]
        public async Task<ActionResult<QuizSubmissionResponse>> GetSubmission(int quizId)
        {
            int userId = await GetCurrentStudentIdAsync();
            var submission = await _quizRepository.GetStudentSubmissionAsync(quizId, userId);

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
            int userId = await GetCurrentStudentIdAsync();

            // Check if student already submitted
            var existingSubmission = await _quizRepository.GetStudentSubmissionAsync(request.QuizId, userId);
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
                StudentId = userId,
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
