
namespace Learning_Academy.DTO
    {
        public class VideoUploadDto
        {
            public int CourseId { get; set; }
            public string Title { get; set; }
            public IFormFile VideoFile { get; set; }
        }
    }

    // DTOs/VideoResponseDto.cs
    namespace Learning_Academy.DTO
    {
        public class VideoResponseDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string FileUrl { get; set; }
            public long FileSize { get; set; }
            public DateTime UploadDate { get; set; }
            public int CourseId { get; set; }
        }
    }

