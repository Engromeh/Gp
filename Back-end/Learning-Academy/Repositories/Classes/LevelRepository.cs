using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Learning_Academy.Repositories.Classes
{
    public class LevelRepository : ILevelRepository
    {
        private readonly LearningAcademyContext _context;

        public LevelRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public Level GetLevelById(int id)
        {
            return _context.Levels.FirstOrDefault(l => l.Id == id);
        }
        public IEnumerable<Level> GetLevelsByCourseId(int courseId)
        {
            return _context.Levels.Where(l => l.CourseId == courseId).ToList();
        }
        public void AddLevel(Level level)
        {
            if (level.CourseId == 0)
                throw new ArgumentException("Level must be associated with a Course.");

            _context.Levels.Add(level);
            _context.SaveChanges();
        }

        public void UpdateLevel(Level level)
        {
            _context.Levels.Update(level);
            _context.SaveChanges();
        }

        public void DeleteLevel(int id)
        {
            var level = _context.Levels.Find(id);
            if (level != null)
            {
                _context.Levels.Remove(level);
                _context.SaveChanges();
            }
        }
    }
}
