namespace Learning_Academy.DTO
{
    public class StudentThatEnrollmentWtithInstructorDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<string> Courses { get; set; } = new();
    }
}
