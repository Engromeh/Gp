using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Learning_Academy.Repositories.Classes
{
    public class InstructorRepository:IInstructorRepostory
    {
      private readonly LearningAcademyContext _context;
        public InstructorRepository(LearningAcademyContext context)
        {
            _context = context;
        }
        public IEnumerable<Instructor> GetAllInstructors()
        {
            return _context.Instructors;
        }

        public Instructor GetByInstructorId(int id)
        {
            return _context.Instructors.SingleOrDefault(e => e.Id == id);
        }

        public void AddInstructor(Instructor instructor)
        {
           _context.Instructors.Add(instructor);
            _context.SaveChanges();
        }

        public void UpdateInstructor(Instructor instructor)
        {
            var inst = _context.Instructors.Find(instructor.Id);
            if (inst == null)
            {
                return;
            }
            
            inst.Id = instructor.Id;
            inst.userName = instructor.userName;
            inst.Email = instructor.Email;
            _context.Instructors.Update(inst);
            _context.SaveChanges(true);
        }
        public void DeleteInstructor(int id)
        {
            try
            {
                var instructor = _context.Instructors.Find(id);
                if (instructor == null)
                {
                    throw new KeyNotFoundException("instructor not found.");
                }
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }catch (KeyNotFoundException e)
            {
                
            }

        }
        public IEnumerable<Course> GetIstructorCourses(int id)
        {
            var courses=_context.Courses.Where(c => c.InstructorId == id).ToList();
            return courses;
        }
        public IEnumerable<Course> GetByCourseName(int InstructorId,string courseName)
        {
            var courses=_context.Courses.Where(c=>c.InstructorId==InstructorId &&c.CourseName.Contains(courseName)).ToList();
            return courses;
        }

    }
}
