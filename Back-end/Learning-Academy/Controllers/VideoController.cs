using Azure;
using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;
        private readonly LearningAcademyContext _context;

        public VideoController(IVideoRepository videoRepository, LearningAcademyContext context)
        {
            _videoRepository = videoRepository;
            _context = context;

        }

        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetVideos()
        {

            var videos = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .Select(v => new
                {
                    v.Id,
                    v.Title,
                    v.ContentType,
                    VideoUrl = v.VideoPath, //  المسار اللي اتحفظ فيه الفيديو
                    LevelId = v.LevelId,
                    LevelName = v.Level != null ? v.Level.Name : null,
                    CourseId = v.CourseId,
                    CourseName = v.Course != null ? v.Course.CourseName : null

                })
               .ToList();

            return Ok(videos);
        }

        [HttpGet("{id}")]
        public IActionResult GetVideoById(int id)
        {

            var video = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .FirstOrDefault(v => v.Id == id);

            if (video == null)
            return NotFound($"video with ID {id} not found.");
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", video.VideoPath.TrimStart('/'));
            long videoSizeKB = 0;

            if (System.IO.File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                videoSizeKB = fileInfo.Length / 1024;
            }


            var result = new
            {
                video.Id,
                video.Title,
                video.ContentType,
                VideoUrl = video.VideoPath,
                VideoSizeKB = videoSizeKB,
                LevelId = video.LevelId,
                LevelName = video.Level?.Name,
                CourseId = video.CourseId,
                CourseName = video.Course?.CourseName
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] VideoDto videoDto)
        {

            if (!ModelState.IsValid) //VideoDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (videoDto == null)
                return BadRequest("❌ Video data is required.");

            var allowedVideoTypes = new[] { "video/mp4", "video/mpeg", "video/ogg", "video/webm", "video/3gpp" };
            var allowedExtensions = new[] { ".mp4", ".mpeg", ".ogg", ".webm", ".3gp" };

            var contentType = videoDto.VideoFile.ContentType.ToLower();
            var extension = Path.GetExtension(videoDto.VideoFile.FileName).ToLower();

            if (!allowedVideoTypes.Contains(contentType) || !allowedExtensions.Contains(extension))
                return BadRequest("❌ Invalid file type. Please upload a valid video format (mp4, webm, etc).");

            // التحقق من Title  
            if (string.IsNullOrWhiteSpace(videoDto.Title) ||
                videoDto.Title.ToLower() == "string" ||
                videoDto.Title.ToLower() == "null")
            {
                return BadRequest("❌ Title is required and cannot be 'string' or 'null'.");
            }

            if (videoDto.VideoFile == null)
                return BadRequest("❌ Video file is required.");

            //اللي بدخل في فيديوهات موجود ف الداتا بيز LevelId التأكد إن  
            if (videoDto.LevelId != null && !_context.Levels.Any(l => l.Id == videoDto.LevelId.Value))
                return BadRequest($"Level with ID {videoDto.LevelId} does not exist.");

            //اللي بدخل في فيديوهات موجود ف الداتا بيز CourseId التأكد إن  
            if (videoDto.CourseId != null && !_context.Courses.Any(c => c.Id == videoDto.CourseId.Value))
                return BadRequest($"Course with ID {videoDto.CourseId} does not exist.");

            //string التأكد أن واحد من الاتنين على الأقل موجود ومش 
            // ماينفعش الفيديو يبقى مش تابع لأي كورس أو ليفل.
            if ((videoDto.LevelId == null && videoDto.CourseId == null) ||
                (videoDto.LevelId?.ToString().ToLower() == "string" && videoDto.CourseId?.ToString().ToLower() == "string"))
            {
                return BadRequest("❌ You must provide either a valid CourseId or LevelId.");
            }

            // 📝 حفظ الفيديو فعليًا
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var videoPath = Path.Combine("wwwroot/videos", uniqueFileName);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), videoPath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // تأكد إن الفولدر موجود

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await videoDto.VideoFile.CopyToAsync(stream);
            }


            var video = new Video
            {
                Title = videoDto.Title,
                //Url = videoDto.Url,
                ContentType = contentType,
                VideoPath = $"/videos/{uniqueFileName}", // 🆕 احنا خزنا المسار بس
                LevelId = videoDto.LevelId,
                CourseId = videoDto.CourseId
            };

            _videoRepository.AddVideo(video);
            await _context.SaveChangesAsync();

            var videoWithDetails = _context.Videos
            .Include(v => v.Level)
            .Include(v => v.Course)
            .FirstOrDefault(v => v.Id == video.Id);

            return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, new
            {
                videoWithDetails.Id,
                videoWithDetails.Title,
                videoWithDetails.ContentType,
                VideoUrl = videoWithDetails.VideoPath,
                LevelId = videoWithDetails.LevelId,
                LevelName = videoWithDetails.Level?.Name,
                CourseId = videoWithDetails.CourseId,
                CourseName = videoWithDetails.Course?.CourseName
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(int id, [FromForm] VideoDto videoDto)
        {
            if (!ModelState.IsValid) //VideoDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (videoDto == null)
                return BadRequest("❌ Video data is required.");
                   
            //مش فاضي وإنه موجود في الداتا VideoId بيز تأكد إن
            var existingVideo = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .FirstOrDefault(v => v.Id == id);

            if (existingVideo == null)
                return NotFound($"❌ Video with ID {id} not found.");

            // التحقق من Title و Url
            if (string.IsNullOrWhiteSpace(videoDto.Title) ||
                videoDto.Title.ToLower() == "string" ||
                videoDto.Title.ToLower() == "null")
            {
                return BadRequest("❌ Title is required and cannot be 'string' or 'null'.");
            }

            if (videoDto.VideoFile == null)
                return BadRequest("❌ Video file is required.");

            //اللي بدخل في فيديوهات موجود ف الداتا بيز LevelId التأكد إن  
            if (videoDto.LevelId != null && !_context.Levels.Any(l => l.Id == videoDto.LevelId.Value))
                return BadRequest($"Level with ID {videoDto.LevelId} does not exist.");

            //اللي بدخل في فيديوهات موجود ف الداتا بيز CourseId التأكد إن  
            if (videoDto.CourseId != null && !_context.Courses.Any(c => c.Id == videoDto.CourseId.Value))
                return BadRequest($"Course with ID {videoDto.CourseId} does not exist.");

            //string التأكد أن واحد من الاتنين على الأقل موجود ومش 
            // ماينفعش الفيديو يبقى مش تابع لأي كورس أو ليفل.
            if ((videoDto.LevelId == null && videoDto.CourseId == null) ||
                (videoDto.LevelId?.ToString().ToLower() == "string" && videoDto.CourseId?.ToString().ToLower() == "string"))
            {
                return BadRequest("❌ You must provide either a valid CourseId or LevelId.");
            }

            var Video = _videoRepository.GetVideoById(id);
            if (Video == null)
                return NotFound($"❌ Video with ID {id} not found.");

            existingVideo.Title = videoDto.Title;
            existingVideo.LevelId = videoDto.LevelId;
            existingVideo.CourseId = videoDto.CourseId;

            if (videoDto.VideoFile != null)
            {
                var allowedVideoTypes = new[] { "video/mp4", "video/mpeg", "video/ogg", "video/webm", "video/3gpp" };
                var allowedExtensions = new[] { ".mp4", ".mpeg", ".ogg", ".webm", ".3gp" };

                var contentType = videoDto.VideoFile.ContentType.ToLower();
                var extension = Path.GetExtension(videoDto.VideoFile.FileName).ToLower();

                if (!allowedVideoTypes.Contains(contentType) || !allowedExtensions.Contains(extension))
                    return BadRequest("❌ Invalid file type. Please upload a valid video format (mp4, webm, etc).");


                // 🆕 حفظ الفيديو الجديد
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var videoPath = Path.Combine("wwwroot/videos", uniqueFileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), videoPath);

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await videoDto.VideoFile.CopyToAsync(stream);
                }

                existingVideo.VideoPath = $"/videos/{uniqueFileName}";
                existingVideo.ContentType = contentType;
            }

                _videoRepository.UpdateVideo(existingVideo);
                await _context.SaveChangesAsync();
           
                var updatedVideo = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .FirstOrDefault(v => v.Id == existingVideo.Id);

            return Ok(new
            {
                updatedVideo.Id,
                updatedVideo.Title,
                updatedVideo.ContentType,
                VideoUrl = updatedVideo.VideoPath,
                updatedVideo.LevelId,
                LevelName = updatedVideo.Level?.Name,
                updatedVideo.CourseId,
                CourseName = updatedVideo.Course?.CourseName
            });

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            var video = _context.Videos.FirstOrDefault(v => v.Id == id);
            if (video == null)
                return NotFound();
            // 🧹 نحذف الملف من السيرفر

            var filePath = Path.Combine("wwwroot", video.VideoPath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _videoRepository.DeleteVideo(id);
            return Ok($"Video with ID {id} has been deleted successfully.");
        }
    }
}
