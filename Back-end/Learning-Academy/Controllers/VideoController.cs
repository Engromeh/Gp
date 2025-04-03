using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;

        public VideoController(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Video>> GetVideos()
        {
            var videos = _videoRepository.GetAllVideos();
            return Ok(videos);
        }

        [HttpGet("{id}")]
        public IActionResult GetVideoById(int id)
        {
            var video = _videoRepository.GetVideoById(id);

            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }

        [HttpPost]
        public IActionResult AddVideo([FromBody] VideoDto videoDto)
        {
            if (videoDto == null)
            {
                return BadRequest("Admindata is required.");
            }

            var video = new Video
            {
                Title = videoDto.Title,
                CourseId = videoDto.CourseId,

            };

            _videoRepository.AddVideo(video);

            return CreatedAtAction(nameof(GetVideoById), new { id = video.Id }, video);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] VideoDto videoDto)
        {
            if (videoDto == null) return BadRequest();

            var existingVideo = _videoRepository.GetVideoById(id);
            if (existingVideo == null) return NotFound();

            existingVideo.Title = videoDto.Title;
            existingVideo.CourseId = videoDto.CourseId;
            


            _videoRepository.UpdateVideo(existingVideo);

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteVideo(int id)
        {
            var existingAdmin = _videoRepository.GetVideoById(id);
            if (existingAdmin == null) return NotFound();

            _videoRepository.DeleteVideo(id);
            return Ok($"Video with ID {id} has been deleted successfully.");
        }
    }
}
