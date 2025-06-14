using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Models.QuizModels;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Classes
{
    public class QuizRepository : IQuizRepository
    {
        private readonly LearningAcademyContext _context;

        public QuizRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _context.Quizzes.FindAsync(id);
        }

        public async Task<Quiz> GetQuizWithQuestionsByIdAsync(int id)
        {
            return await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByCourseIdAsync(int courseId)
        {
            return await _context.Quizzes
            .Where(q => q.CourseId == courseId)
            .ToListAsync();
        }

       public async Task<Quiz> AddQuizAsync(Quiz quiz)
        {
            //quiz.CreatedAt = DateTime.UtcNow;
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);
        }

        async Task<Question> IQuizRepository.AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions
        .Include(q => q.Options)
        .FirstOrDefaultAsync(q => q.Id == id);

            if (question != null)
            {
                // First delete all answer options to maintain referential integrity
                _context.Options.RemoveRange(question.Options);

                // Then delete the question
                _context.Questions.Remove(question);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<QuizSubmission> GetSubmissionByIdAsync(int id)
        {
            return await _context.QuizSubmissions
            .Include(s => s.StudentAnswers)
                .ThenInclude(a => a.SelectedOption)
            .Include(s => s.StudentAnswers)
                .ThenInclude(a => a.Question)
            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<QuizSubmission>> GetSubmissionsByQuizIdAsync(int quizId)
        {
            return await _context.QuizSubmissions
            .Where(s => s.QuizId == quizId)
            .Include(s => s.Student)
            .Include(s => s.StudentAnswers)
            .ToListAsync();
        }

        public async Task<QuizSubmission> GetStudentSubmissionAsync(int quizId, int studentId)
        {
            return await _context.QuizSubmissions
            .Include(s => s.StudentAnswers)
                .ThenInclude(a => a.SelectedOption)
            .Include(s => s.StudentAnswers)
                .ThenInclude(a => a.Question)
            .FirstOrDefaultAsync(s => s.QuizId == quizId && s.StudentId == studentId);
        }

        public async Task<QuizSubmission> AddSubmissionAsync(QuizSubmission submission)
        {
            submission.SubmissionTime = DateTime.UtcNow;
            _context.QuizSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            return submission;
        }
        public async Task<List<QuizResponse>> GetQuizzesByCourseIdsAsync(List<int> courseIds)
        {
            return await _context.Quizzes
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
        }




    }
}
