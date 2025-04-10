using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Url { get; set; }= null!;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = null!;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Course")]
        public virtual int CourseId {get; set; }
        public virtual Course Course { get; set; } = null!;

    }
}
