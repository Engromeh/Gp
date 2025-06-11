using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Classes
{
    public class StudentRepository :IStudentRepository
    {
        private readonly LearningAcademyContext _context;
        public StudentRepository(LearningAcademyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students.Include(i => i.User)        
            .ThenInclude(u => u.Profile)
                .Include(s => s.StudentEnrollments)
                    .ThenInclude(se => se.Course)
                .ToListAsync();

            return students.Select(s => new StudentDto
            {
                ImageFile = s.User?.Profile?.ProfileImageUrl,
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                EnrollmentCourses = s.StudentEnrollments != null
                    ? s.StudentEnrollments.Select(se => new CourseADDto
                    {
                        CourseName = se.Course.CourseName
                    }).ToList()
                    : new List<CourseADDto>()
            });
        }


        public async Task< StudentDto> GetByStudentId(int id)
        {
            var student = await _context.Students.Include(i => i.User)      
            .ThenInclude(u => u.Profile)
             .Include(s => s.StudentEnrollments)
             .ThenInclude(se => se.Course)
             .Where(s => s.Id == id)
             .Select(s => new StudentDto
             {
                 ImageFile = s.User.Profile.ProfileImageUrl,
                 Id = s.Id,
                 UserName = s.UserName,
                 Email = s.Email,
                 EnrollmentCourses = s.StudentEnrollments != null
                     ? s.StudentEnrollments.Select(se => new CourseADDto
                     {
                         CourseName = se.Course.CourseName
                         
                     }).ToList()
                     : new List<CourseADDto>(),
                 
             })
             .SingleOrDefaultAsync();
            return student;
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

        public void UpdateStudent(Task<StudentDto> student)
        {
            var stud= _context.Students.Find(student.Id);
            if(stud == null)
            {
                return;
            }
            
            stud.UserName= student.Result.UserName;
            stud.Email= student.Result.Email;
          //  stud.Admin= student.Admin;
          //  stud.AdminId= student.AdminId;
          //  stud.Massages= student.Massages;
        //    stud.Country= student.Country;
         //   stud.Rates= student.Rates;
        //    stud.StudentEnrollments = student.StudentEnrollments;
            _context.Students.Update(stud);
            _context.SaveChanges();
        }

        public Student GetByUserId(string userId)
        {
            return _context.Students.FirstOrDefault(s => s.UserId == userId);
        }
    }
}
