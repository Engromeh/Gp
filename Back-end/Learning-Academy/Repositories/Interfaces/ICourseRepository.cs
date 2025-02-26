using Learning_Academy.Models;
using Learning_Academy.DTO;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Course GetByCourseId(int id);
        void AddCourse(Course course);
        void DeleteCourse(int id);
        void UpdateCourse(Course course);






    }
}
