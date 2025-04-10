using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Learning_Academy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
// Controllers/VideosController.cs
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IVideoService videoService, ILogger<VideoController> logger)
        {
            _videoService = videoService;
            _logger = logger;
        }

        [HttpPost]
        [DisableRequestSizeLimit] // Or set a specific limit
        public async Task<ActionResult<VideoResponseDto>> UploadVideo([FromForm] VideoUploadDto videoUploadDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var video = await _videoService.UploadVideoAsync(videoUploadDto);
                return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, video);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid video upload attempt");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video");
                return StatusCode(500, "An error occurred while uploading the video");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoResponseDto>> GetVideo(int id)
        {
            var video = await _videoService.GetVideoAsync(id);
            if (video == null)
                return NotFound();

            return Ok(video);
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<VideoResponseDto>>> GetVideosByCourse(int courseId)
        {
            var videos = await _videoService.GetVideosByCourseAsync(courseId);
            return Ok(videos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            try
            {
                var result = await _videoService.DeleteVideoAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting video");
                return StatusCode(500, "An error occurred while deleting the video");
            }
        }
    }
}