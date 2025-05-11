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
                InstructorId = instructor.Id,
                Levels = new List<Level>()
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

            // Update main data
            existingCourse.CourseName = courseDto.CourseName;
            existingCourse.CourseDescription = courseDto.CourseDescription;

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
            _context.Videos.RemoveRange(existingCourse.Levels.SelectMany(l => l.Videos));
            _context.Levels.RemoveRange(existingCourse.Levels);
            _context.Courses.Remove(existingCourse);

            await _context.SaveChangesAsync();

            return Ok($"✅ Course with ID {id} and all associated levels/videos deleted.");
        }

    }
}