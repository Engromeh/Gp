using Learning_Academy.DTO;
using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICourseRatingRepository
    {
        // Basic CRUD operations
        Task<CourseRating> AddRatingAsync(CourseRating rating);
        Task<CourseRating> GetRatingByIdAsync(int id);
        Task<IEnumerable<CourseRating>> GetRatingsForCourseAsync(int courseId);
        // Task<CourseRating> UpdateRatingAsync(CourseRating rating);
        Task<CourseRating> UpdateRatingAsync(CreateRatingDto updateDto, int ratingId);
        Task DeleteRatingAsync(int id);

        // Specific queries
        Task<bool> HasStudentRatedCourseAsync(int studentId, int courseId);
        Task<CourseRating> GetStudentRatingForCourseAsync(int studentId, int courseId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);

        // Statistics
        Task<double> GetAverageRatingAsync(int courseId);
        Task<int> GetTotalRatingsAsync(int courseId);

    }
}
