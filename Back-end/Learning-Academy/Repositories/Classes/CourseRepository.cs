using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;
using Learning_Academy.DTO;
using Learning_Academy.Repositories.Interfaces;


namespace Learning_Academy.Repositories.Classes
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LearningAcademyContext _context;

        public CourseRepository(LearningAcademyContext context)
        {
            _context = context;
        }
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses;
        }
        public Course GetByCourseId(int id)
        {

            return _context.Courses.SingleOrDefault(c => c.Id == id);
        }
        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();
        }


        public void UpdateCourse(Course course)
        {
            var CourseEntity = _context.Courses.Find(course.Id);
            if (CourseEntity == null)
            {
                return;
            }
            CourseEntity.CourseName = course.CourseName;
            CourseEntity.AdminId = course.AdminId;
            CourseEntity.CourseDescription = course.CourseDescription;
            CourseEntity.CertificateId = course.CertificateId;
            CourseEntity.InstructorId = course.InstructorId;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }


    }
}
