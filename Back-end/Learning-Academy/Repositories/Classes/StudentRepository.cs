using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;

namespace Learning_Academy.Repositories.Classes
{
    public class StudentRepository :IStudentRepository
    {
        private readonly LearningAcademyContext _context;
        public StudentRepository(LearningAcademyContext context)
        {
            _context = context;
        }
        public IEnumerable<Student> GetAllStudents()
        {
           return _context.Students;
        }

        public Student GetByStudentId(int id)
        {
            return _context.Students.SingleOrDefault(e => e.Id == id);
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var Student =_context.Students.Find(id);
            if (Student != null)
            {
                _context.Students.Remove(Student);
                _context.SaveChanges(true);
            }
            else
            {
                throw new KeyNotFoundException("student not found");
            }
        }

        public void UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
