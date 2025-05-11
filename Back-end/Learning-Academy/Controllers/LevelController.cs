using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Linq;
using System.Security.Policy;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelRepository _levelRepository;
        private readonly LearningAcademyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LevelController(ILevelRepository levelRepository, IWebHostEnvironment webHostEnvironment, LearningAcademyContext context)
 
        {
            _levelRepository = levelRepository;
            _webHostEnvironment = webHostEnvironment;
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
                CourseName = level.Course?.CourseName,
                Videos = level.Videos?.Select(v => new {
                    v.Id,
                    v.Title,
                    //v.Url
                    v.ContentType,
                    VideoPath = v.VideoPath

                })
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
                Videos = level.Videos?.Select(v => new
                {
                    v.Id,
                    v.Title,
                    //v.Url
                    v.ContentType,
                    VideoPath = v.VideoPath
                })
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddLevel([FromForm] CreateLevelWithVideosDto levelDto)
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
           
            if (levelDto.VideoFiles.Count != levelDto.VideoTitles.Count)
                return BadRequest("❌ Number of video files must match number of titles.");
            
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var level = new Level
            {
                Name = levelDto.Name,
                CourseId = levelDto.CourseId,
                Videos = new List<Video>()
            };

            for (int i = 0; i < levelDto.VideoFiles.Count; i++)
            {
                var file = levelDto.VideoFiles[i];
                var title = levelDto.VideoTitles[i];

                if (string.IsNullOrWhiteSpace(title) || title.ToLower() == "string" || title.ToLower() == "null")
                    return BadRequest($"❌ Title is invalid cant by null or string.");

                if (file == null)
                    return BadRequest($"❌ Video file at index {i} is required.");

                var allowedVideoTypes = new[] { "video/mp4", "video/mpeg", "video/ogg", "video/webm", "video/3gpp" };
                var allowedExtensions = new[] { ".mp4", ".mpeg", ".ogg", ".webm", ".3gp" };

                var contentType = file.ContentType.ToLower();
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedVideoTypes.Contains(contentType) || !allowedExtensions.Contains(extension))
                    return BadRequest($"❌ Invalid file, Only video formats are allowed is: .mp4, .mpeg, .ogg, .webm, .3gp");

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                level.Videos.Add(new Video
                {
                    Title = title,
                    ContentType = file.ContentType,
                    VideoPath = $"/videos/{uniqueFileName}", // فقط المسار النسبي
                    CourseId = levelDto.CourseId
                });
            }

            _levelRepository.AddLevel(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLevelById), new { id = level.Id }, new
            {
                level.Id,
                level.Name,
                level.CourseId,
                CourseName = _context.Courses.FirstOrDefault(c => c.Id == level.CourseId)?.CourseName,
                Videos = level.Videos.Select(v => new
                {
                    v.Title,
                    v.VideoPath,
                    v.ContentType,
                })
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevel(int id, [FromForm] CreateLevelWithVideosDto levelDto)
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

            // 🧹 حذف الفيديوهات القديمة من السيرفر
            foreach (var video in existingLevel.Videos)
            {
                if (!string.IsNullOrEmpty(video.VideoPath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", video.VideoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
            }

            // 🗑️ حذف الفيديوهات من الداتا
            _context.Videos.RemoveRange(existingLevel.Videos);
            existingLevel.Videos.Clear();


               for (int i = 0; i < levelDto.VideoFiles.Count; i++)
               {

                if (i >= levelDto.VideoTitles.Count)
                    return BadRequest("❌ Number of video files must match number of titles.");


                var file = levelDto.VideoFiles[i];
                    var title = levelDto.VideoTitles[i];


                    if (string.IsNullOrWhiteSpace(title) || title.ToLower() == "string" || title.ToLower() == "null")
                        return BadRequest($"❌ Title is invalid cant by null or string.");

                    if (file == null)
                        return BadRequest($"❌ Video file at index {i} is required.");

                    
                    var contentType = file.ContentType.ToLower();
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    var allowedVideoTypes = new[] { "video/mp4", "video/mpeg", "video/ogg", "video/webm", "video/3gpp" };
                    var allowedExtensions = new[] { ".mp4", ".mpeg", ".ogg", ".webm", ".3gp" };

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                    Directory.CreateDirectory(uploadPath); // تأكد الفولدر موجود

                    if (!allowedVideoTypes.Contains(contentType) || !allowedExtensions.Contains(extension))
                        return BadRequest($"❌ Invalid file, Only video formats are allowed is: .mp4, .mpeg, .ogg, .webm, .3gp");

                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var fullPath = Path.Combine(uploadPath, uniqueFileName);
                   
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    existingLevel.Videos.Add(new Video
                    {
                        Title = title,
                        VideoPath = $"/videos/{uniqueFileName}",
                        ContentType = file.ContentType,
                        CourseId = levelDto.CourseId,
                        LevelId = existingLevel.Id
                    });
               }
            

            _levelRepository.UpdateLevel(existingLevel);
            await _context.SaveChangesAsync();

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
                Videos = updatedLevel.Videos.Select(v => new
                {
                    v.Id,
                    v.Title,
                    v.ContentType,
                    v.VideoPath
                })
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLevel(int id)
        {
            var existingLevel = _context.Levels
            .Include(l => l.Videos)
            .FirstOrDefault(l => l.Id == id);

            if (existingLevel == null)
                return NotFound("Level not found");
           
            // 🧹 نحذف كل الفيديوهات من السيرفر
            foreach (var video in existingLevel.Videos)
            {
                if (!string.IsNullOrEmpty(video.VideoPath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", video.VideoPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }

            // ❌ نحذف الفيديوهات من الداتا بيز
            _context.Videos.RemoveRange(existingLevel.Videos);

            // ❌ نحذف الليفل نفسه
            _context.Levels.Remove(existingLevel);

            _context.SaveChanges();

            _levelRepository.DeleteLevel(id);
            return Ok("Level deleted successfully");
        }
    }
}