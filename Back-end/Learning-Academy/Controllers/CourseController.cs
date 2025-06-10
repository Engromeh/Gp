using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Collections.Generic;



namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {


        private readonly ICourseRepository _courseRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly UserManager<User> _userManager;
        private readonly LearningAcademyContext _context;



        public CourseController(ICourseRepository courseRepository, ILevelRepository levelRepository, UserManager<User> userManager, LearningAcademyContext context)
        {
            _courseRepository = courseRepository;
            _levelRepository = levelRepository;
            _userManager = userManager;
            _context = context;

        }

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Levels)
                    .ThenInclude(l => l.Videos)
                .ToList();

            var result = courses.Select(course => new
            {
                course.Id,
                course.CourseName,
                course.CourseDescription,
                ImageUrl = course.ImagePath,
                course.Category,
                course.InstructorId,
                InstructorName = course.Instructor?.UserName,
                Levels = course.Levels?.Select(level => new
                {
                    level.Id,
                    level.Name,
                    Videos = level.Videos?.Select(v => new
                    {
                        v.Id,
                        v.Title,
                        VideoPath = v.VideoPath

                    }).ToList()
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetByCourseId(int id)
        {

            var course = _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Levels)
                    .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
                return NotFound($"❌ Course with ID {id} not found.");

            var result = new
            {
                course.Id,
                course.CourseName,
                course.CourseDescription,
                ImageUrl = course.ImagePath,
                course.Category,
                course.InstructorId,
                InstructorName = course.Instructor?.UserName,
                Levels = course.Levels?.Select(level => new
                {
                    level.Id,
                    level.Name,
                    Videos = level.Videos?.Select(v => new
                    {
                        v.Id,
                        v.Title,
                        VideoPath = v.VideoPath

                    }).ToList()
                }).ToList()
            };

            return Ok(result);

        }
       
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromForm] CourseDto courseDto)
        {

            if (courseDto == null)
                return BadRequest("❌ Course data is required.");

            if (!ModelState.IsValid) //CourseDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(courseDto.CourseName) ||
                courseDto.CourseName.ToLower() == "string" ||
                courseDto.CourseName.ToLower() == "null")
            {
                return BadRequest("❌ CourseName is required and cannot be 'string' or null.");
            }

            if (string.IsNullOrWhiteSpace(courseDto.CourseDescription) ||
                courseDto.CourseDescription.ToLower() == "string" ||
                courseDto.CourseDescription.ToLower() == "null")
            {
                return BadRequest("❌ CourseDescription is required and cannot be 'string' or null.");
            }

            if (string.IsNullOrWhiteSpace(courseDto.levelName) ||
                courseDto.levelName.ToLower() == "string" ||
                courseDto.levelName.ToLower() == "null")
            {
                return BadRequest("❌ levelName is required and cannot be 'string' or null.");
            }

            if (string.IsNullOrWhiteSpace(courseDto.Category) ||
                courseDto.Category.ToLower() == "string" ||
                courseDto.Category.ToLower() == "null")
            {
                return BadRequest("❌ Category is required and cannot be 'string' or null.");
            }

            if (courseDto.ImageFile == null || courseDto.ImageFile.Length == 0)
                return BadRequest("❌ Image is required.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(courseDto.ImageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("❌ Only image files (.jpg, .jpeg, .png, .gif) are allowed.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await courseDto.ImageFile.CopyToAsync(stream);
            }

            var imagePath = $"/images/{uniqueFileName}";
           

            string? userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("❌ User email not found in token.");
            }

            // ابحث عن اليوزر بالإيميل
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return Unauthorized("❌ User not found.");
            }

            // هات الإنستراكتور بناءً على اليوزر
            var instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.UserId == user.Id);
            if (instructor == null)
            {
                return Unauthorized("❌ Instructor not found in database.");
            }

            // إنشاء الكورس
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                CourseDescription = courseDto.CourseDescription,
                ImagePath = imagePath,
                Category = courseDto.Category,
                InstructorId = instructor.Id,
                Levels = new List<Level>(),

            };

            course.Levels.Add(new Level
            {
                Name = courseDto.levelName
            });

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetByCourseId), new { id = course.Id }, new
            {
                InstructorId = instructor.Id,
                InstructorName = instructor.User?.UserName,
                Courseid=course.Id,
                course.CourseName,
                course.CourseDescription,
                course.ImagePath,
                course.Category,
                Level = course.Levels.Select(l => new 
                {
                    Level_id=l.Id,
                    Level_Name=l.Name 
                }),
               
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseDto courseDto)
        {
            if (courseDto == null)
                return BadRequest("❌ Course data is required.");
           
            if (!ModelState.IsValid) //CourseDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            var existingCourse = await _context.Courses
                .Include(c => c.Levels)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCourse == null)
                return NotFound($"❌ Course with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(courseDto.CourseName) ||
                courseDto.CourseName.ToLower() == "string" || courseDto.CourseName.ToLower() == "null")
                return BadRequest("❌ CourseName is required and cannot be 'string' or null.");

            if (string.IsNullOrWhiteSpace(courseDto.CourseDescription) ||
                courseDto.CourseDescription.ToLower() == "string" || courseDto.CourseDescription.ToLower() == "null")
                return BadRequest("❌ CourseDescription is required and cannot be 'string' or null.");

            if (string.IsNullOrWhiteSpace(courseDto.levelName) || courseDto.levelName.ToLower() is "string" or "null")
                return BadRequest("❌ levelName is required and cannot be 'string' or null.");
           
            if (string.IsNullOrWhiteSpace(courseDto.Category) ||  courseDto.Category.ToLower() == "string" ||   courseDto.Category.ToLower() == "null")
                return BadRequest("❌ Category is required and cannot be 'string' or null.");

            if (courseDto.ImageFile != null && courseDto.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(courseDto.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("❌ Only image files (.jpg, .jpeg, .png, .gif) are allowed.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await courseDto.ImageFile.CopyToAsync(stream);
                }

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(existingCourse.ImagePath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingCourse.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                existingCourse.ImagePath = $"/images/{uniqueFileName}";
            }

            // Update main data
            existingCourse.CourseName = courseDto.CourseName;
            existingCourse.CourseDescription = courseDto.CourseDescription;
            existingCourse.Category = courseDto.Category;

            var firstLevel = existingCourse.Levels.FirstOrDefault();
            if (firstLevel != null)
            {
                firstLevel.Name = courseDto.levelName;
            }
            else
            {
                existingCourse.Levels.Add(new Level
                {
                    Name = courseDto.levelName
                });
            }

            _context.Courses.Update(existingCourse);
            await _context.SaveChangesAsync();

            //Response بظبط شكل لداتا اللي هترجع في الـ 
            var response = new
            {
                InstructorId = existingCourse.InstructorId,
                InstructorName = existingCourse.Instructor?.UserName,
                Courseid = existingCourse.Id,
                existingCourse.CourseName,
                existingCourse.CourseDescription,
                existingCourse.ImagePath,
                existingCourse.Category,

                Level = existingCourse.Levels.Select(l => new
                {
                    Level_id = l.Id,
                    Level_Name = l.Name
                })

            };

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var existingCourse = _context.Courses
                .Include(c => c.Levels)
                .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == id);

            if (existingCourse == null)
                return NotFound("❌ Course not found.");

            // حذف ملفات الفيديو (لو في)
            foreach (var video in existingCourse.Levels.SelectMany(l => l.Videos))
            {
                if (!string.IsNullOrEmpty(video.VideoPath))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", video.VideoPath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }

            if (!string.IsNullOrEmpty(existingCourse.ImagePath))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingCourse.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Videos.RemoveRange(existingCourse.Levels.SelectMany(l => l.Videos));
            _context.Levels.RemoveRange(existingCourse.Levels);
            _context.Courses.Remove(existingCourse);

            await _context.SaveChangesAsync();

            return Ok($"✅ Course with ID {id} and all associated levels/videos deleted.");
        }

        //يجيب الكورسات على حسب اهتمامات Student
        [Authorize(Roles = "Student")]
        [HttpGet("suggested-courses")]
        public async Task<IActionResult> GetSuggestedCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("❌ User not authorized.");

            var student = await _context.Students
                .Include(s => s.Interests)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return NotFound("❌ Student not found.");

            var interests = student.Interests
                .Select(i => i.Category.Trim().ToLower())
                .Distinct()
                .ToList();

            if (!interests.Any())
                return NotFound("❌ No interests found. Please select your interests to see suggested courses.");

            // 🔁 قاموس الكلمات المرتبطة بكل اهتمام
            var keywordMap = new Dictionary<string, List<string>>
    {
        { "web", new List<string> { "web", "frontend", "backend", "html", "css", "javascript", "react", "web development" } },
        { "frontend", new List<string> { "frontend", "html", "css", "react", "javascript" } },
        { "backend", new List<string> { "backend", "asp", "api", "server", "c#" } },
        { "mobile", new List<string> { "mobile", "flutter", "android", "ios", "kotlin", "swift" } },
        { "graphic", new List<string> { "graphic", "design", "photoshop", "illustrator", "ui", "ux", "visual" } },
        { "ai", new List<string> { "ai", "artificial", "machine learning", "ml", "deep learning", "neural" } },
        { "full stack", new List<string> { "full stack", "frontend", "backend", "api", "web" } }
    };

            // 🧠 تحويل اهتمامات الطالب لكلمات موسعة
            var expandedKeywords = interests
                .SelectMany(interest =>
                    keywordMap.ContainsKey(interest)
                        ? keywordMap[interest]
                        : new List<string> { interest } // fallback لو مفيش mapping
                )
                .Distinct()
                .ToList();

            // 🔍 البحث عن كورسات فيها أي من الكلمات الموسعة
            var suggestedCourses = _context.Courses
                .Include(c => c.Instructor)
                .Where(course =>
                    expandedKeywords.Any(keyword =>
                        course.Category.ToLower().Contains(keyword) ||
                        course.CourseName.ToLower().Contains(keyword) ||
                        course.CourseDescription.ToLower().Contains(keyword)
                    ))
                .Select(c => new
                {
                    c.Id,
                    c.CourseName,
                    c.CourseDescription,
                    c.Category,
                    Instructor = c.Instructor != null ? c.Instructor.User.UserName : "Unknown"
                })
                .ToList();

            if (!suggestedCourses.Any())
                return NotFound("❌ No courses found matching your interests yet. Try updating them!");

            return Ok(suggestedCourses);
        }



        //كورسات حسب Category معين
        [HttpGet("search with Category")]
        public IActionResult SmartSearch([FromQuery] string Category)
        {
            if (string.IsNullOrWhiteSpace(Category))
                return BadRequest("❌ Search keyword is required.");

            var keywordMap = new Dictionary<string, List<string>>
    {
        { "web development", new List<string> { "web", "frontend", "backend", "html", "css", "react", "asp" } },
        { "frontend", new List<string> { "frontend", "html", "css", "react" } },
        { "backend", new List<string> { "backend", "asp", "api", "server" } },
        { "mobile", new List<string> { "flutter", "android", "ios", "mobile" } },
        { "full stack", new List<string> { "frontend", "backend", "web", "api" } },
        { "design", new List<string> { "ui", "ux", "graphic", "photoshop" } }
    };

            var searchTerm = Category.Trim().ToLower();
            var expandedKeywords = keywordMap.ContainsKey(searchTerm)
                ? keywordMap[searchTerm]
                : new List<string> { searchTerm }; // لو مش موجود في الماب، نستخدم الكلمة زي ما هي

            var results = _context.Courses
                .Include(c => c.Instructor)
                .Where(c =>
                    expandedKeywords.Any(kw =>
                        c.Category.ToLower().Contains(kw) ||
                        c.CourseName.ToLower().Contains(kw) ||
                        c.CourseDescription.ToLower().Contains(kw)
                    ))
                .Select(c => new
                {
                    c.Id,
                    c.CourseName,
                    c.CourseDescription,
                    c.Category,
                    Instructor = c.Instructor != null ? c.Instructor.User.UserName : "Unknown"
                })
                .ToList();

            if (!results.Any())
                return NotFound($"❌ No courses found related to '{Category}'.");

            return Ok(results);
        }



    }
}