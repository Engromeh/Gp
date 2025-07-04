﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.Models
{
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string VideoPath { get; set; } = null!; // هنا هيتخزن اسم أو مسار الفيديو داخل wwwroot/videos
        [Required]
        public string ContentType { get; set; } = null!; // النوع (video/mp4 مثلاً)


        [ForeignKey("Level")]
        public virtual int? LevelId { get; set; }
        public virtual Level? Level { get; set; } = null!;
        
        [ForeignKey("Course")]
        public virtual int? CourseId { get; set; }
        public virtual Course? Course { get; set; } = null!;
    }
}
