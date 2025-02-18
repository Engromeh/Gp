using Learning_Academy.Models;

namespace Learning_Academy.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
        Course GetByCourseId(int id);
        
    }
}
