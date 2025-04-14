using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Learning_Academy.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public VideoService(
            IVideoRepository videoRepository,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _videoRepository = videoRepository;
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<VideoResponseDto> GetVideoAsync(int id)
        {
            var video = await _videoRepository.GetVideoByIdAsync(id);
            if (video == null) return null;

            return new VideoResponseDto
            {
                Id = video.Id,
                Title = video.Title,
                FileUrl = $"/{video.Url.Replace("\\", "/")}",
                FileSize = video.FileSize,
                UploadDate = video.UploadDate,
                CourseId = video.CourseId
            };
        }

        public async Task<IEnumerable<VideoResponseDto>> GetVideosByCourseAsync(int courseId)
        {
            var videos = await _videoRepository.GetVideosByCourseIdAsync(courseId);
            return videos.Select(v => new VideoResponseDto
            {
                Id = v.Id,
                Title = v.Title,
                FileUrl = $"/{v.Url.Replace("\\", "/")}",
                FileSize = v.FileSize,
                UploadDate = v.UploadDate,
                CourseId = v.CourseId
            });
        }

        public async Task<VideoResponseDto> UploadVideoAsync(VideoUploadDto videoUploadDto)
        {
            // Validate file
            if (videoUploadDto.VideoFile == null || videoUploadDto.VideoFile.Length == 0)
                throw new ArgumentException("No video file uploaded");

            // Validate file size (e.g., 100MB max)
            var defaultMax = 100L * 1024 * 1024; // 100MB
            var maxFileSize = _configuration.GetSection("VideoSettings").Exists()
                             ? _configuration.GetValue<long>("VideoSettings:MaxFileSizeBytes")
                             : defaultMax;
            if (videoUploadDto.VideoFile.Length > maxFileSize)
                throw new ArgumentException($"File size exceeds the maximum limit of {maxFileSize / (1024 * 1024)}MB");

            // Validate file extension
            var allowedExtensions = _configuration.GetSection("VideoSettings:AllowedExtensions").Get<string[]>()
                                    ?? new[] { ".mp4", ".mov", ".avi", ".mkv" };
            var fileExtension = Path.GetExtension(videoUploadDto.VideoFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException($"Only the following extensions are allowed: {string.Join(", ", allowedExtensions)}");

            // Create upload directory if it doesn't exist
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Media", "Uploads", "Videos",
                DateTime.Now.ToString("yyyy-MM"));
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoUploadDto.VideoFile.CopyToAsync(fileStream);
            }

            // Create video entity
            var video = new Video
            {
                Title = videoUploadDto.Title,
                FileName = uniqueFileName,
                Url = Path.Combine("uploads", "videos", uniqueFileName),
                FileSize = videoUploadDto.VideoFile.Length,
                ContentType = videoUploadDto.VideoFile.ContentType,
                CourseId = videoUploadDto.CourseId
            };

            // Save to database
            var createdVideo = await _videoRepository.AddVideoAsync(video);

            // Return response DTO
            return new VideoResponseDto
            {
                Id = createdVideo.Id,
                Title = createdVideo.Title,
                FileUrl = $"/{createdVideo.Url.Replace("\\", "/")}",
                FileSize = createdVideo.FileSize,
                UploadDate = createdVideo.UploadDate,
                CourseId = createdVideo.CourseId
            };
        }
        //public async Task<bool> DeleteVideoAsync(int id)
        //{
        //    // Get the video first to access the file path
        //    var video = await _videoRepository.GetVideoByIdAsync(id);
        //    if (video == null)
        //        return false;

        //    // Delete the physical file
        //    var filePath = Path.Combine(_environment.ContentRootPath, video.Url);
        //    if (File.Exists(filePath))
        //    {
        //        File.Delete(filePath);
        //    }

        //    // Delete the database record
        //    return await _videoRepository.DeleteVideoAsync(id);
        //}

        
        public async Task<bool> DeleteVideoAsync11(int id)
        {
            // Get video record from database
            var video = await _videoRepository.GetVideoByIdAsync(id);
            if (video == null) return false;

            // Construct the full file path
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Media", "Uploads", "Videos");
            var filePath = Path.Combine(uploadsFolder, video.FileName);

            // Delete physical file
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    // File doesn't exist but we'll still delete the DB record
                    return await _videoRepository.DeleteVideoAsync(id);
                }
            }
            catch (Exception)
            {
                // Continue with DB deletion even if file deletion fails
            }

            // 4. Delete database record
            return await _videoRepository.DeleteVideoAsync(id);
        }
    }
    
}