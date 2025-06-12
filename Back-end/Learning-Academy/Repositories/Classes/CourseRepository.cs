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
                    Id=c.Id,
                    ImageUrl = c.ImagePath,
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
                    Id=c.Id,
                    ImageUrl = c.ImagePath,
                    CourseName = c.CourseName,
                    InstructorName = c.Instructor.UserName, 
                    CountOfEnrollment = c.Enrollments.Count()
                })
                .FirstOrDefaultAsync();
        }


        public int AddCourse(CourseDto dto)
        {
            string? imagePath = null;
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(dto.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException("❌ Only image files (.jpg, .jpeg, .png, .gif) are allowed.");

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.ImageFile.CopyTo(stream);
                }

                imagePath = $"/images/{uniqueFileName}";
            }


            var course = new Course
            {
                CourseName = dto.CourseName,
                CourseDescription = dto.CourseDescription,
                ImagePath = imagePath,
                Category = dto.Category,
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
            courseEntity.Category = dto.Category;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(dto.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException("❌ Only image files (.jpg, .jpeg, .png, .gif) are allowed.");

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var newImagePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    dto.ImageFile.CopyTo(stream);
                }

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(courseEntity.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, courseEntity.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                courseEntity.ImagePath = $"/images/{uniqueFileName}";
            }

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
            course.ImagePath = updatedCourse.ImageUrl;
            

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<int>> GetCourseIdsByInstructorIdAsync(int instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => c.Id)
                .ToListAsync();
        }



        public void DeleteCourse(int id)
        {
            var course = _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            // حذف الفيديوهات
            foreach (var video in course.Levels.SelectMany(l => l.Videos))
            {
                if (!string.IsNullOrEmpty(video.VideoPath))
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, video.VideoPath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }

            // حذف الصورة
            if (!string.IsNullOrEmpty(course.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, course.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Videos.RemoveRange(course.Levels.SelectMany(l => l.Videos));
            _context.Levels.RemoveRange(course.Levels);
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

