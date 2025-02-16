using Learning_Academy.Models;

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
        public Course GetCourseByCourseId(int id) {
            return _context.Courses.FirstOrDefault();
        }
        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }
        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }
        public void DeleteCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

        }

        public Course GetByCourseId(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteCourse(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
