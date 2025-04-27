using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Classes
{
    public class CourseRatingRepository : ICourseRatingRepository
    {
        private readonly LearningAcademyContext _context;
        public CourseRatingRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public async Task<CourseRating> AddRatingAsync(CourseRating rating)
        {
            _context.CourseRatings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<double> GetAverageRatingAsync(int courseId)
        {
            return await _context.CourseRatings
            .Where(r => r.CourseId == courseId)
            .AverageAsync(r => (double?)r.RatingValue) ?? 0.0;
        }

        public async Task<List<CourseRating>> GetRatingsForCourseAsync(int courseId)
        {
            return await _context.CourseRatings
            .Where(r => r.CourseId == courseId)
            .Include(r => r.Student)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        }

        public async Task<int> GetTotalRatingsAsync(int courseId)
        {
            return await _context.CourseRatings
           .CountAsync(r => r.CourseId == courseId);
        }

        public async Task<bool> HasStudentRatedCourseAsync(int studentId, int courseId)
        {
            return await _context.CourseRatings
            .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _context.CourseEnrollment
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
