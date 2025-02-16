using Learning_Academy.Models;

namespace Learning_Academy.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Course GetByCourseId(int id);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        void AddCourse(Course course);
    }
}
