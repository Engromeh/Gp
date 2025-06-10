using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Academy.DTO
{
    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseDescription { get; set; }
        public string? CourseImageUrl { get; set; }
        public string? CourseCategory { get; set; }
        public string? CourseLevel { get; set; }
        public int CourseDuration { get; set; } // Duration in hours
        public DateTime EnrollmentDate { get; set; }
        public int CourseInstructorId { get; set; }
        public string? CourseInstructorName { get; set; }
        public string Status { get; set; } = "Pending"; // Default status
        public int CourseRating { get; set; }
    }
}
