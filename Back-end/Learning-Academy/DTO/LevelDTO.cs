using Learning_Academy.DTO;
using Learning_Academy.Models;
using System.ComponentModel.DataAnnotations;

public class LevelDto
{

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "CourseId is required.")]
    public int CourseId { get; set; } 

    public virtual List<VideoDto>? Videos { get; set; }
    
    

}
