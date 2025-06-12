namespace Learning_Academy.DTO
{
    public class AdminCourseDto
    {

        public int Id { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public int CountOfEnrollment { get; set; }
        public string? ImageUrl  { get; set; }
    }
}
