using Learning_Academy.DTO;
using Learning_Academy.Models;
using System.ComponentModel.DataAnnotations;

public class LevelDto
{

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "CourseId is required.")]
    public int CourseId { get; set; }

    
}
public class CreateLevelWithVideosDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public int CourseId { get; set; }

    public List<IFormFile>? VideoFiles { get; set; } = new();

    public List<string>? VideoTitles { get; set; } = new();
}