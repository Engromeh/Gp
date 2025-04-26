using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<Quiz> GetQuizWithQuestionsByIdAsync(int id);
        Task<IEnumerable<Quiz>> GetQuizzesByCourseIdAsync(int courseId);
        Task<Quiz> AddQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);

        Task<Question> GetQuestionByIdAsync(int id);
        Task<Question> AddQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);

        // Submission operations
        Task<QuizSubmission> GetSubmissionByIdAsync(int id);
        Task<List<QuizSubmission>> GetSubmissionsByQuizIdAsync(int quizId);
        Task<QuizSubmission> GetStudentSubmissionAsync(int quizId, int studentId);
        Task<QuizSubmission> AddSubmissionAsync(QuizSubmission submission);
    }
}
