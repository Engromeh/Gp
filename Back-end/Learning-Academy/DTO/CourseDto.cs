using Learning_Academy.DTO;
using System.ComponentModel.DataAnnotations;

public class CourseDto
{
    [Required(ErrorMessage = "Course Name is required.")]
    public string CourseName { get; set; } = null!;

    [Required(ErrorMessage = "Course Description is required.")]
    public string CourseDescription { get; set; } = null!;
  
    [Required(ErrorMessage = "level Name is required.")]
    public string levelName { get; set; } = null!;

}


