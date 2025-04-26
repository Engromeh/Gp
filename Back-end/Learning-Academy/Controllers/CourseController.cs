using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;



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
                .Include(c => c.Admin)
                .Include(c => c.Certificate)
                .Include(c => c.Levels)
                    .ThenInclude(l => l.Videos)
                .ToList();

            var result = courses.Select(course => new
            {
                course.Id,
                course.CourseName,
                course.CourseDescription,
                course.CourseDateTime,
                CertificateId = course.CertificateId,
                course.AdminId,
                AdminName = $"{course.Admin?.FirstName} {course.Admin?.LastName}",
                course.InstructorId,
                InstructorName = $"{course.Instructor?.FirstName} {course.Instructor?.LastName}",
                Levels = course.Levels?.Select(level => new
                {
                    level.Id,
                    level.Name,
                    Videos = level.Videos?.Select(v => new
                    {
                        v.Id,
                        v.Title,
                        v.Url
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
                .Include(c => c.Admin)
                .Include(c => c.Certificate)
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
                course.CourseDateTime,
                CertificateId = course.CertificateId,
                course.AdminId,
                AdminName = $"{course.Admin?.FirstName} {course.Admin?.LastName}",
                course.InstructorId,
                InstructorName = $"{course.Instructor?.FirstName} {course.Instructor?.LastName}",
                Levels = course.Levels?.Select(level => new
                {
                    level.Id,
                    level.Name,
                    Videos = level.Videos?.Select(v => new
                    {
                        v.Id,
                        v.Title,
                        v.Url
                    }).ToList()
                }).ToList()
            };

            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid) //CourseDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (courseDto == null)
                return BadRequest("❌ Course data is required.");

            // التحقق من CourseName و CourseDescription
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

            // بيتأكد إن الشهادة اللي دخّلها المستخدم موجودة في الداتابيز قبل ما نربطها بالكورس.
            if (courseDto.CertificateId != null && !_context.Certificate.Any(c => c.Id == courseDto.CertificateId))
                return BadRequest($"❌ Certificate with ID {courseDto.CertificateId} does not exist.");


            //CourseDateTime  تحقق من الفورمات بتاع ScheduleText
            if (!string.IsNullOrWhiteSpace(courseDto.CourseDateTime))
            {
                if (!Regex.IsMatch(courseDto.CourseDateTime, @"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)\s*:?\s+\d{1,2}:\d{2}\s+(AM|PM)$"))
                {
                    return BadRequest("❌ CourseDateTime must be like 'Tuesday 07:00 PM'");
                }
            }
            // JWT Token من ال UserId ده بيطلع الـ 
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("❌ User ID not found in token.");
            }

            //بتاعه بيساوي اللي جاي من التوكن UserId لى الإنستراكتور اللي الـ  Instructors هنا بتروح تدور في جدول var instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.UserId == userId);
            var instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.UserId == userId);
            if (instructor == null)
            {
                return Unauthorized("❌ Instructor not found in database.");
            }

            // إنشاء الكورس
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                CourseDescription = courseDto.CourseDescription,
                CourseDateTime = courseDto.CourseDateTime,
                AdminId = courseDto.AdminId,
                InstructorId = instructor.Id,
                CertificateId = courseDto.CertificateId,
                Levels = new List<Level>()
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            // ربط الليفيل لو اتبعتت
            if (courseDto.Levels != null)
            {
                foreach (var levelDto in courseDto.Levels)
                {
                    //  التحقق من اسم الليفل
                    if (string.IsNullOrWhiteSpace(levelDto.Name) ||
                         levelDto.Name.ToLower() == "string" ||
                         levelDto.Name.ToLower() == "null")
                    {
                        return BadRequest("❌ level Name is required and cannot be 'string' or 'null'.");
                    }

                    var level = new Level
                    {
                        Name = levelDto.Name,
                        CourseId = course.Id,
                        Videos = new List<Video>()
                    };
                    // ربط الفيدوهات لو اتبعتت

                    if (levelDto.Videos != null)
                    {
                        foreach (var videoDto in levelDto.Videos)
                        {
                            //URL و Title تأكد إن كل فيديو فيه   

                            if (string.IsNullOrWhiteSpace(videoDto.Title) ||
                                 videoDto.Title.ToLower() == "string" ||
                                 videoDto.Title.ToLower() == "null")
                            {
                                return BadRequest("❌ Title is required and cannot be 'string' or 'null'.");
                            }

                            if (string.IsNullOrWhiteSpace(videoDto.Url) ||
                                videoDto.Url.ToLower() == "string" ||
                                videoDto.Url.ToLower() == "null")
                            {
                                return BadRequest("❌ Url is required and cannot be 'string' or 'null'.");
                            }
                            level.Videos.Add(new Video
                            {
                                Title = videoDto.Title,
                                Url = videoDto.Url,
                                CourseId = course.Id,// ربط الفيديو بالكورس اللي جاي من الليفل
                                LevelId = level.Id  // هيتسجل بعد الحفظ
                            });
                        }
                    }
                    _context.Levels.Add(level);
                }
                _context.SaveChanges();
            }

            // استرجاع الكورس بالتفاصيل
            var Course = _context.Courses
                .Include(c => c.Admin)
                .Include(c => c.Instructor)
                .Include(c => c.Certificate)
                .Include(c => c.Levels)
                    .ThenInclude(l => l.Videos)
                .FirstOrDefault(c => c.Id == course.Id);

            //Response بظبط شكل لداتا اللي هترجع في الـ 
            var response = new
            {
                course.Id,
                course.CourseName,
                course.CourseDescription,
                course.CourseDateTime,
                course.InstructorId,
                InstructorName = $"{Course.Instructor?.FirstName} {Course.Instructor?.LastName}",
                course.AdminId,
                AdminName = $"{Course.Admin?.FirstName} {Course.Admin?.LastName}",
                course.CertificateId,
                Levels = course.Levels.Select(l => new
                {
                    l.Id,
                    l.Name,
                    Videos = l.Videos?.Select(v => new
                    {
                        v.Title
                    ,
                        v.Url
                    }).ToList()
                })
            };

            return CreatedAtAction(nameof(GetByCourseId), new { id = course.Id }, response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid) //CourseDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (courseDto == null)
                return BadRequest("❌ Course data is required.");


            var existingCourse = _context.Courses
                 .Include(c => c.Levels)
                     .ThenInclude(l => l.Videos)
                 .Include(c => c.Instructor)
                 .Include(c => c.Admin)
                 .FirstOrDefault(c => c.Id == id);

            if (existingCourse == null)
                return NotFound($"❌ Course with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(courseDto.CourseName) ||
                courseDto.CourseName.ToLower() == "string" || courseDto.CourseName.ToLower() == "null")
                return BadRequest("❌ CourseName is required and cannot be 'string' or null.");

            if (string.IsNullOrWhiteSpace(courseDto.CourseDescription) ||
                courseDto.CourseDescription.ToLower() == "string" || courseDto.CourseDescription.ToLower() == "null")
                return BadRequest("❌ CourseDescription is required and cannot be 'string' or null.");

            if (!string.IsNullOrWhiteSpace(courseDto.CourseDateTime))
            {
                if (!Regex.IsMatch(courseDto.CourseDateTime, @"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)\s*:?\s+\d{1,2}:\d{2}\s+(AM|PM)$"))
                    return BadRequest("❌ CourseDateTime must be like 'Tuesday 07:00 PM'");
            }

            if (courseDto.CertificateId != null && !_context.Certificate.Any(c => c.Id == courseDto.CertificateId))
                return BadRequest($"❌ Certificate with ID {courseDto.CertificateId} does not exist.");


            // Update main data
            existingCourse.CourseName = courseDto.CourseName;
            existingCourse.CourseDescription = courseDto.CourseDescription;
            existingCourse.CourseDateTime = courseDto.CourseDateTime;
            existingCourse.CertificateId = courseDto.CertificateId;
            existingCourse.AdminId = courseDto.AdminId;

            // 🧹 احذف المستويات القديمة قبل إضافة الجديدة
            _context.Levels.RemoveRange(existingCourse.Levels);
            if (courseDto.Levels != null)
            {
                foreach (var levelDto in courseDto.Levels)
                {
                    if (string.IsNullOrWhiteSpace(levelDto.Name) ||
                        levelDto.Name.ToLower() == "string" || levelDto.Name.ToLower() == "null")
                        return BadRequest("❌ Level Name is required and cannot be 'string' or null.");

                    var level = new Level
                    {
                        Name = levelDto.Name,
                        CourseId = existingCourse.Id,
                        Videos = new List<Video>()
                    };

                    if (levelDto.Videos != null)
                    {
                        foreach (var videoDto in levelDto.Videos)
                        {
                            if (string.IsNullOrWhiteSpace(videoDto.Title) || videoDto.Title.ToLower() == "string" || videoDto.Title.ToLower() == "null")
                                return BadRequest("❌ Title is required and cannot be 'string' or null.");

                            if (string.IsNullOrWhiteSpace(videoDto.Url) || videoDto.Url.ToLower() == "string" || videoDto.Url.ToLower() == "null")
                                return BadRequest("❌ Url is required and cannot be 'string' or null.");

                            level.Videos.Add(new Video
                            {
                                Title = videoDto.Title,
                                Url = videoDto.Url,
                                CourseId = existingCourse.Id
                            });
                        }
                    }

                    existingCourse.Levels.Add(level);
                }
            }

           _context.Courses.Update(existingCourse);
            await _context.SaveChangesAsync();


            //Response بظبط شكل لداتا اللي هترجع في الـ 
            var response = new
            {
                existingCourse.Id,
                existingCourse.CourseName,
                existingCourse.CourseDescription,
                existingCourse.CourseDateTime,
                InstructorId = existingCourse.InstructorId,
                InstructorName = $"{existingCourse.Instructor?.FirstName} {existingCourse.Instructor?.LastName}",
                AdminId = existingCourse.AdminId,
                AdminName = $"{existingCourse.Admin?.FirstName} {existingCourse.Admin?.LastName}",
                CertificateId = existingCourse.CertificateId,
                Levels = existingCourse.Levels.Select(l => new
                {
                    l.Id,
                    l.Name,
                    Videos = l.Videos.Select(v => new
                    {
                        v.Title,
                        v.Url
                    }).ToList()
                })
            };

            return Ok(response);

        }
    
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var existingCourse = _courseRepository.GetByCourseId(id);

            if (existingCourse == null)
            {
                return NotFound("Course not found.");
            }
            else
            {

                _courseRepository.DeleteCourse(id);

                return Ok($"Course with ID {id} has been deleted successfully.");
            }
        }






    }
}