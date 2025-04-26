using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Level
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required]
        public string Name { get; set; } = null!;

        [ForeignKey("Course")]
        public virtual int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public virtual List<Video>? Videos { get; set; }
    }
}
