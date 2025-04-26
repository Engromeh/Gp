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

            var result = new
            {
                video.Id,
                video.Title,
                video.Url,
                LevelId = video.LevelId,
                LevelName = video.Level?.Name,
                CourseId = video.CourseId,
                CourseName = video.Course?.CourseName

            };

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddVideo([FromBody] VideoDto videoDto)
        {

            if (!ModelState.IsValid) //VideoDto دي عشان لو اليوزر نسي يدخل حاجه من اللي ف ال
                return BadRequest(ModelState);

            if (videoDto == null)
                return BadRequest("❌ Video data is required.");

            // التحقق من Title و Url
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

            var video = new Video
            {
                Title = videoDto.Title,
                Url = videoDto.Url,
                LevelId = videoDto.LevelId,
                CourseId = videoDto.CourseId
            };

            _videoRepository.AddVideo(video); 
            _context.SaveChanges();

            // نرجع الفيديو من الداتا بيز بكل تفاصيله بعد ما اتسجل
            var videoWithDetails = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .FirstOrDefault(v => v.Id == video.Id);

            //Response بظبط شكل لداتا اللي هترجع في الـ 
            var response = new
            {
                videoWithDetails.Id,
                videoWithDetails.Title,
                videoWithDetails.Url,
                LevelId = videoWithDetails.LevelId,
                LevelName = videoWithDetails.Level?.Name,
                CourseId = videoWithDetails.CourseId,
                CourseName = videoWithDetails.Course?.CourseName
            };

            return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] VideoDto videoDto)
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

            if (string.IsNullOrWhiteSpace(videoDto.Url) ||
                videoDto.Url.ToLower() == "string" ||
                videoDto.Url.ToLower() == "null")
            {
                return BadRequest("❌ Url is required and cannot be 'string' or 'null'.");
            }
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

            Video.Title = videoDto.Title;
            Video.Url = videoDto.Url;
            Video.LevelId = videoDto.LevelId;
            Video.CourseId = videoDto.CourseId;

            _videoRepository.UpdateVideo(Video);
            _context.SaveChanges();

            // جلب البيانات المحدثة بعد التعديل
            var updatedVideo = _context.Videos
                .Include(v => v.Level)
                .Include(v => v.Course)
                .FirstOrDefault(v => v.Id == Video.Id);

            //Response بظبط شكل لداتا اللي هترجع في الـ 
            var response = new
            {
                updatedVideo.Id,
                updatedVideo.Title,
                updatedVideo.Url,
                LevelId = updatedVideo.LevelId,
                LevelName = updatedVideo.Level?.Name,
                CourseId = updatedVideo.CourseId,
                CourseName = updatedVideo.Course?.CourseName
            };

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            var existingVideo = _videoRepository.GetVideoById(id);
            if (existingVideo == null)
                return NotFound();

            _videoRepository.DeleteVideo(id);
            return Ok($"Video with ID {id} has been deleted successfully.");
        }
    }
}
