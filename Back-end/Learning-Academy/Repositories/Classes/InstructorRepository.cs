using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Classes
{
    public class InstructorRepository
    {
      private readonly LearningAcademyContext _context;
        public InstructorRepository(LearningAcademyContext context)
        {
            _context = context;
        }
    }
}
