using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        IEnumerable<StudentRatingCourse> GetAllRating();
        StudentRatingCourse GetRatingById(int ratingId);
        void AddRate(StudentRatingCourse rating);
        void UpdateRate(StudentRatingCourse rating);
        void DeleteRate(int ratingId);

    }
}
