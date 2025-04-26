using Learning_Academy.DTO;
using System.ComponentModel.DataAnnotations;

public class CourseDto
{
    [Required(ErrorMessage = "CourseName is required.")]
    public string CourseName { get; set; } = null!;

    [Required(ErrorMessage = "CourseDescription is required.")]
    public string CourseDescription { get; set; } = null!;
    public int AdminId { get; set; }
    public int? CertificateId { get; set; }

    [Required(ErrorMessage = "CourseDescription is required.")]
    public string CourseDateTime { get; set; } = null!; //"Tuesday 07:00 PM" وقت الكورس   

    public List<LevelDto>? Levels { get; set; }
}


