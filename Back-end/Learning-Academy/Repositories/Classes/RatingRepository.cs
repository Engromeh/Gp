using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LearningAcademyContext _context;
        public RatingRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentRatingCourse> GetAllRating()
        {
            return _context.StudentRatingCourse;
        }

        public StudentRatingCourse GetRatingById(int ratingId)
        {
            return _context.StudentRatingCourse.SingleOrDefault(e => e.Id == ratingId);
        }

        public void UpdateRate(StudentRatingCourse rating)
        {
            _context.StudentRatingCourse.Update(rating);
            _context.SaveChanges();
        }
        public void AddRate(StudentRatingCourse rating)
        {
            _context.StudentRatingCourse.Add(rating);
            _context.SaveChanges();
        }

        public void DeleteRate(int ratingId)
        {
            var Rate = _context.StudentRatingCourse.Find(ratingId);
            if (Rate != null) 
            { 
                _context.StudentRatingCourse.Remove(Rate);
                _context.SaveChanges();
            }
        }
    }
}
