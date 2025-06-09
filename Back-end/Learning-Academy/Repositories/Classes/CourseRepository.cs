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
        public async Task<IEnumerable<AdminCourseDto>> GetAllCoursesForAdminAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Select(c => new AdminCourseDto
                {
                    CourseName = c.CourseName,
                    InstructorName = c.Instructor.UserName, 
                    CountOfEnrollment = c.Enrollments.Count()
                })
                .ToListAsync();
        }


        public Course GetByCourseId(int id)
        {
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == id);
        }
        public async Task<AdminCourseDto?> GetCourseForAdminByIdAsync(int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Select(c => new AdminCourseDto
                {
                    CourseName = c.CourseName,
                    InstructorName = c.Instructor.UserName, 
                    CountOfEnrollment = c.Enrollments.Count()
                })
                .FirstOrDefaultAsync();
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
        public async Task<bool> UpdateCourseAsync(int courseId, AdminEditCourseDto updatedCourse)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return false;

            course.CourseName = updatedCourse.CourseName;
            course.InstructorId = updatedCourse.InstructorId;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
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
        public async Task<bool> DeleteCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return false;

            if (course.Enrollments != null && course.Enrollments.Any())
                _context.CourseEnrollment.RemoveRange(course.Enrollments);

            if (course.CourseRatinds != null && course.CourseRatinds.Any())
                _context.CourseRatings.RemoveRange(course.CourseRatinds);

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Course> GetByIdWithInstructorAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public Course GetCourseByName(string name)
        {
            return _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.CourseName==name);
        }

    }
}

