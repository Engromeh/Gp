using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LearningAcademyContext _context;

        public CourseRepository(LearningAcademyContext context) {
            _context = context;
        }
        public IEnumerable<Course> GetAllCourses() { 
            return _context.Courses;
        }
        public Course GetByCourseId(int id) {
            return _context.Courses.FirstOrDefault();
        }
        





    }
}
