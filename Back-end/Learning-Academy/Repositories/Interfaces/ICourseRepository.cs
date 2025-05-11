
using Learning_Academy.Models;
using Learning_Academy.DTO;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Course GetByCourseId(int id);
        int AddCourse(CourseDto dto);
        void UpdateCourse(CourseDto dto, int courseId);

        void DeleteCourse(int id);
        Task<Course> GetByIdWithInstructorAsync(int id);






    }
}