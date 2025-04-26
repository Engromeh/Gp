using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface ILevelRepository
    {
        IEnumerable<Level> GetLevelsByCourseId(int courseId);
        Level? GetLevelById(int id);
        void AddLevel(Level level);
        void UpdateLevel(Level level);
        void DeleteLevel(int id);
    }
}
