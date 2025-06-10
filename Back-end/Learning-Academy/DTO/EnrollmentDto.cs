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
        public DateTime EnrollmentDate { get; set; }
        public string? Status { get; set; }
        public int CourseInstructorId { get; set; }
        public string? CourseInstructorName { get; set; }
    }
}
