using System.ComponentModel.DataAnnotations;

namespace Learning_Academy.DTO
{
    public class VideoDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = null!;
        
        [Required(ErrorMessage = "Url is required.")]
        public string Url { get; set; } = null!;

        public int? LevelId { get; set; }
        public int? CourseId { get; set; }
    }

}
