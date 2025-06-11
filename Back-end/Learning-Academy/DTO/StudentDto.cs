using Learning_Academy.Models;
namespace Learning_Academy.DTO

{
    public class StudentDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<CourseADDto> EnrollmentCourses { get; set; }


    }
}
