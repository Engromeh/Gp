
using Learning_Academy.Models;
using Learning_Academy.DTO;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Task<IEnumerable<AdminCourseDto>> GetAllCoursesForAdminAsync();
        Course GetByCourseId(int id);
        Task<AdminCourseDto?> GetCourseForAdminByIdAsync(int courseId);
        int AddCourse(CourseDto dto);
        void UpdateCourse(CourseDto dto, int courseId);
        Task<bool> UpdateCourseAsync(int courseId, AdminEditCourseDto updatedCourse);
        Course GetCourseByName(string name);

        Task<List<int>> GetCourseIdsByInstructorIdAsync(int instructorId);
        void DeleteCourse(int id);
        Task<Course> GetByIdWithInstructorAsync(int id);
        Task DeleteCourseAsync(int id);








    }
}