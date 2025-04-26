using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelRepository _levelRepository;
        private readonly LearningAcademyContext _context;

          public LevelController(ILevelRepository levelRepository, LearningAcademyContext context)
        {
            _levelRepository = levelRepository;
            _context = context;
        }


        // ✅ GET: Get all levels with their videos and course info
        [HttpGet]
        public IActionResult GetLevels()
        {
            var levels = _context.Levels
                .Include(l => l.Course)
                .Include(l => l.Videos)
                .ToList();

            var result = levels.Select(level => new
            {
                level.Id,
                level.Name,
                level.CourseId,
                CourseName = level.Course.CourseName,
                Videos = level.Videos?.Select(v => new { v.Id, v.Title, v.Url })
            });

            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetLevelById(int id)

        {
            var level = _context.Levels
                .Include(l => l.Course)
                .Include(l => l.Videos)
                .FirstOrDefault(l => l.Id == id);

            if (level == null) 
            return NotFound($"Level with ID {id} not found.");

            var result = new
            {
                level.Id,
                level.Name,
                level.CourseId,
                CourseName = level.Course?.CourseName,
                Videos = level.Videos?.Select(v => new { v.Id, v.Title, v.Url })
            };

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddLevel([FromBody] LevelDto levelDto)
        {
            if (!ModelState.IsValid)//levelDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (levelDto == null)
                return BadRequest("❌ Invalid level data.");
           
            //  التحقق من اسم الليفل
            if (string.IsNullOrWhiteSpace(levelDto.Name) || levelDto.Name.ToLower() == "string")
                return BadRequest("❌ Level Name is required and cannot be 'string' or 'null'.");

            //  التحقق من CourseId
            if (levelDto.CourseId <= 0 || levelDto.CourseId.ToString().ToLower() == "string")
                return BadRequest("❌ CourseId must be a valid number more than 0 and cannot be 'string' or null.");

            //  تأكد الكورس موجود
            if (!_context.Courses.Any(c => c.Id == levelDto.CourseId))
                return BadRequest($"❌ Course with ID {levelDto.CourseId} does not exist.");

            if (levelDto.CourseId == null)
                return BadRequest("Course ID is required.");

            var level = new Level
            {
                Name = levelDto.Name,
                CourseId = levelDto.CourseId,
                Videos = new List<Video>()
            };
            // نضيف الفيديوهات لو اتبعتت

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
                        CourseId = levelDto.CourseId // ربط الفيديو بالكورس اللي جاي من الليفل
                                                     // LevelId هيتضاف تلقائي بعد ما الليفل يتسجل
                    });
                }
            }

            _levelRepository.AddLevel(level);
            _context.SaveChanges();

            //Response بظبط شكل لداتا اللي هترجع في الـ 
            return CreatedAtAction(nameof(GetLevelById), new { id = level.Id }, new
            {
                level.Id,
                level.Name,
                level.CourseId,
                CourseName = _context.Courses.FirstOrDefault(c => c.Id == level.CourseId)?.CourseName,
                Videos = level.Videos.Select(v => new
                {
                    v.Title,
                    v.Url
                })
            });           
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLevel(int id, [FromBody] LevelDto levelDto)
        {
            if (!ModelState.IsValid) //levelDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (levelDto == null)
                return BadRequest("Level data is required.");

            //مش فاضي وإنه موجود في الداتا LevelId بيز تأكد إن
            var existingLevel = _context.Levels
                .Include(l => l.Videos)
                .FirstOrDefault(l => l.Id == id);

            if (existingLevel == null)
                return NotFound($"Level with ID {id} not found.");
            
            //null أو string تأكد الاسم بتاع الليفل مش فاضي أو   ""
            if (string.IsNullOrWhiteSpace(levelDto.Name) || levelDto.Name.ToLower() == "string")
                return BadRequest("❌ Level Name is required and cannot be 'string' or 'null'.");


            // ✅ تأكد CourseId مش بيساوي 0 أو "string"
            if (levelDto.CourseId <= 0 || levelDto.CourseId.ToString().ToLower() == "string")
                return BadRequest("❌ CourseId must be a valid number more than 0 and cannot be 'string' or null.");

            // مش فاضي وإنه موجود في الداتا CourseId بيز تأكد إن
            if (!_context.Courses.Any(c => c.Id == levelDto.CourseId))
                return BadRequest($"Course with ID {levelDto.CourseId} does not exist.");

            existingLevel.Name = levelDto.Name;
            existingLevel.CourseId = levelDto.CourseId;
            existingLevel.Videos = new List<Video>();

            if (levelDto.Videos != null)
            {
                foreach (var videoDto in levelDto.Videos)
                {
                    //لو في فيديوهات، لازم يكون الـ Title و الـ Url مش فاضيين ومش فيهم كلمة "string" كـ default value 
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
                    existingLevel.Videos.Add(new Video
                    {
                        Title = videoDto.Title,
                        Url = videoDto.Url,
                        CourseId = existingLevel.CourseId,
                        LevelId = existingLevel.Id // ربط الفيديو بالليفل الحالي
                    });
                }
            }

            _levelRepository.UpdateLevel(existingLevel);
            _context.SaveChanges();

            // نحمل الداتا تاني بعد السيف عشان الفيديوهات تظهر
            var updatedLevel = _context.Levels
                .Include(l => l.Course)
                .Include(l => l.Videos)
                .FirstOrDefault(l => l.Id == existingLevel.Id);

            var response = new
            {
                updatedLevel.Id,
                updatedLevel.Name,
                updatedLevel.CourseId,
                CourseName = updatedLevel.Course?.CourseName,
                Videos = updatedLevel.Videos?.Select(v => new
                {
                    v.Title,
                    v.Url
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLevel(int id)
        {
            var existingLevel = _levelRepository.GetLevelById(id);
            if (existingLevel == null) return NotFound("Level not found");

            _levelRepository.DeleteLevel(id);
            return Ok("Level deleted successfully");
        }
    }
}
