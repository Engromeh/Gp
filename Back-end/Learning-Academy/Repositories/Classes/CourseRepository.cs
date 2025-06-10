using Learning_Academy.Models;
using Microsoft.EntityFrameworkCore;
using Learning_Academy.DTO;
using Learning_Academy.Repositories.Interfaces;
using Humanizer;

namespace Learning_Academy.Repositories.Classes
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LearningAcademyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseRepository(LearningAcademyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos) // لو عايز تعرض الفيديوهات داخل كل ليفل
                .ToList();
        }

        public Course GetByCourseId(int id)
        {
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == id);
        }

        public int AddCourse(CourseDto dto)
        {
            var course = new Course
            {
                CourseName = dto.CourseName,
                CourseDescription = dto.CourseDescription,
                InstructorId = null, // ممكن تضيفها من dto لو محتاج
                Levels = new List<Level>()

            };

            // إنشاء أول level بناءً على اسم الليفل في CourseDto (بس بدون فيديوهات)
            if (!string.IsNullOrWhiteSpace(dto.levelName))
            {
                var level = new Level
                {
                    Name = dto.levelName,
                    Videos = new List<Video>()
                };

                course.Levels.Add(level);
            }

            _context.Courses.Add(course);
            _context.SaveChanges();

            return course.Id;
        }

        public void UpdateCourse(CourseDto dto, int courseId)
        {
            var courseEntity = _context.Courses
                .Include(c => c.Levels)
                .FirstOrDefault(c => c.Id == courseId);


            if (courseEntity == null)
                throw new Exception("Course not found.");

            courseEntity.CourseName = dto.CourseName;
            courseEntity.CourseDescription = dto.CourseDescription;
            // courseEntity.InstructorId = dto.InstructorId;

            if (!string.IsNullOrWhiteSpace(dto.levelName))
            {
                var firstLevel = courseEntity.Levels.FirstOrDefault();
                if (firstLevel != null)
                {
                    firstLevel.Name = dto.levelName;
                }
                else
                {
                    courseEntity.Levels.Add(new Level
                    {
                        Name = dto.levelName,
                        Videos = new List<Video>()
                    });
                }
            }

            _context.Courses.Update(courseEntity);
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
        public async Task<Course> GetByIdWithInstructorAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}

