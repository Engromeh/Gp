﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Learning_Academy.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; } = null !;
        
        [ForeignKey("Admin")]
        public virtual int? AdminId { get; set; }
        [JsonIgnore]
        public virtual Admin? Admin { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public virtual string? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<StudentInterest> Interests { get; set; } = new List<StudentInterest>();
        public virtual ICollection<Enrollment> StudentEnrollments { get; set; }=new List<Enrollment>();
        public virtual ICollection<CourseRating>Rates { get; set; }=new List<CourseRating>();
        public virtual ICollection<ChatMessage> Massages { get; set; } = new List<ChatMessage>();
       
    } 
}
