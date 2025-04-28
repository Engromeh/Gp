using Learning_Academy.DTO;
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

        public async Task<CourseRating> GetRatingByIdAsync(int id)
        {
            return await _context.CourseRatings
                .Include(r => r.Student)
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<CourseRating>> GetRatingsForCourseAsync(int courseId)
        {
            return await _context.CourseRatings
                .Include(r => r.Student)
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        public async Task<CourseRating> AddRatingAsync(CourseRating rating)
        {
            rating.CreatedAt = DateTime.UtcNow;
            _context.CourseRatings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<CourseRating> UpdateRatingAsync(CreateRatingDto updateDto, int ratingId)
        {
            // 1. Get the existing rating from database (tracked)
            var existingRating = await _context.CourseRatings
                .FirstOrDefaultAsync(r => r.Id == ratingId);

            if (existingRating == null)
            {
                return null;
            }

            // 2. Update only the modifiable fields
            existingRating.RatingValue = updateDto.RatingValue;
            existingRating.Comment = updateDto.Comment;

            // 3. Save changes
            await _context.SaveChangesAsync();

            return existingRating;
        }
        public async Task DeleteRatingAsync(int id)
        {
            var rating = await GetRatingByIdAsync(id);
            if (rating != null)
            {
                _context.CourseRatings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> HasStudentRatedCourseAsync(int studentId, int courseId)
        {
            return await _context.CourseRatings
                .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<CourseRating> GetStudentRatingForCourseAsync(int studentId, int courseId)
        {
            return await _context.CourseRatings
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _context.CourseEnrollment
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<double> GetAverageRatingAsync(int courseId)
        {
            var average = await _context.CourseRatings
                .Where(r => r.CourseId == courseId)
                .AverageAsync(r => (double?)r.RatingValue) ?? 0.0;

            return Math.Round(average, 2);
        }

        public async Task<int> GetTotalRatingsAsync(int courseId)
        {
            return await _context.CourseRatings
                .CountAsync(r => r.CourseId == courseId);
        }

    }
}
