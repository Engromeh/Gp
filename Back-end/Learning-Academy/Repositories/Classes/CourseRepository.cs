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
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .ToList();
        }

        public Course GetByCourseId(int id)
        {

            return _context.Courses.SingleOrDefault(c => c.Id == id);
        }
        public Course GetByCourseIdWithLevelsAndVideos(int id)
        {
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .SingleOrDefault(c => c.Id == id);
        }
        public int AddCourse(Course course)
        {
            if (course.Levels != null && course.Levels.Any())
            {
                foreach (var level in course.Levels)
                {
                    level.Course = course;

                    if (level.Videos != null && level.Videos.Any())
                    {
                        foreach (var video in level.Videos)
                        {
                            video.Level = level;
                        }
                    }
                }
            }

            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.Id;
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
            _context.Courses.Update(CourseEntity);
            _context.SaveChanges();
        }

        public async Task<Course> GetByIdWithInstructorAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

