using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICourseRatingRepository
    {
        Task<CourseRating> AddRatingAsync(CourseRating rating);
        Task<double> GetAverageRatingAsync(int courseId);
        Task<int> GetTotalRatingsAsync(int courseId);
        Task<List<CourseRating>> GetRatingsForCourseAsync(int courseId);
        Task<bool> HasStudentRatedCourseAsync(int studentId, int courseId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        //IEnumerable<CourseRating> GetAllRating();
        //CourseRating GetRatingById(int ratingId);
        //void AddRate(CourseRating rating);
        //void UpdateRate(CourseRating rating);
        //void DeleteRate(int ratingId);

    }
}
