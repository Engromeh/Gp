using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Repositories.Classes
{
    public class InstructorRepository : IInstructorRepostory
    {
        private readonly LearningAcademyContext _context;
        public InstructorRepository(LearningAcademyContext context)
        {
            _context = context;
        }


        public IEnumerable<InstructorDto> GetAllInstructors()
        {
            return _context.Instructors.Include(i => i.User)        
            .ThenInclude(u => u.Profile)
                .Select(i => new InstructorDto
                {
                    ImageFile=i.User.Profile.ProfileImageUrl,
                    Id = i.Id,
                    UserName = i.UserName,
                    Email = i.Email,
                    CountOfCourses = i.Courses.Count()
                })
                .ToList();
        }
        public async Task<Instructor> GetInstructorByIdAsync(int id)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public InstructorDto GetByInstructorId(int id)
        {
            var instructor = _context.Instructors.Include(i => i.User)         
            .ThenInclude(u => u.Profile)
            .Where(i => i.Id == id)
            .Select(i => new InstructorDto
            {
                ImageFile = i.User.Profile.ProfileImageUrl,
                Id = i.Id,
                UserName = i.UserName,
                Email = i.Email,
                CountOfCourses = i.Courses.Count()
            })
            .SingleOrDefault();
            return instructor;
        }
        public async Task DeleteInstructorAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return;

            if (instructor.UserId != null)
            {
                var profile = await _context.Profiles
                    .FirstOrDefaultAsync(p => p.UserId == instructor.UserId);

                if (profile != null)
                {
                    _context.Profiles.Remove(profile);
                    await _context.SaveChangesAsync(); 
                }
            }

            var courseIds = await _context.Courses
                .Where(c => c.InstructorId == id)
                .Select(c => c.Id)
                .ToListAsync();

            if (courseIds.Any())
            {
                var quizIds = await _context.Quizzes
                    .Where(q => courseIds.Contains(q.CourseId))
                    .Select(q => q.Id)
                    .ToListAsync();

                if (quizIds.Any())
                {
                    var questionIds = await _context.Questions
                        .Where(q => quizIds.Contains(q.QuizId.Value))
                        .Select(q => q.Id)
                        .ToListAsync();

                    if (questionIds.Any())
                    {
                        var options = await _context.Options
                            .Where(o => questionIds.Contains(o.QuestionId.Value))
                            .ToListAsync();

                        _context.Options.RemoveRange(options);

                        var questions = await _context.Questions
                            .Where(q => quizIds.Contains(q.QuizId.Value))
                            .ToListAsync();

                        _context.Questions.RemoveRange(questions);
                    }

                    var quizzes = await _context.Quizzes
                        .Where(q => courseIds.Contains(q.CourseId))
                        .ToListAsync();

                    _context.Quizzes.RemoveRange(quizzes);
                }

                var courses = await _context.Courses
                    .Where(c => c.InstructorId == id)
                    .ToListAsync();

                _context.Courses.RemoveRange(courses);
            }

           
            if (instructor.UserId != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == instructor.UserId);

                if (user != null)
                {
                    _context.Users.Remove(user);
                }
            }

           

            
            _context.Instructors.Remove(instructor);

            
            await _context.SaveChangesAsync();
        }

        public void AddInstructor(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            _context.SaveChanges();
        }

        public void UpdateInstructor(InstructorDto instructor)
        {
            var inst = _context.Instructors.Find(instructor.Id);
            if (inst == null)
            {
                return;
            }

            inst.Id = instructor.Id;
            inst.UserName = instructor.UserName;
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
            }
            catch (KeyNotFoundException e)
            {

            }
        }
        public async Task<List<StudentThatEnrollmentWtithInstructorDto>> GetStudentsWithTheirCoursesAsync(string instructorUserId)
        {
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.UserId == instructorUserId);

            if (instructor == null)
                return new List<StudentThatEnrollmentWtithInstructorDto>();

            var studentsWithCourses = await _context.CourseEnrollment
                .Include(e => e.Student).ThenInclude(s => s.User)
                .Include(e => e.Course)
                .Where(e => e.Course.InstructorId == instructor.Id)
                .GroupBy(e => new
                {
                    StudentId = e.StudentId,
                    StudentName = e.Student.User.UserName,
                    Email = e.Student.User.Email,
                    ProfileImageUrl = e.Student.User.Profile.ProfileImageUrl
                })
                .Select(g => new StudentThatEnrollmentWtithInstructorDto
                {
                    StudentId = g.Key.StudentId,
                    StudentName = g.Key.StudentName,
                    Email = g.Key.Email,
                    ProfileImageUrl = g.Key.ProfileImageUrl,
                    Courses = g.Select(e => e.Course.CourseName).Distinct().ToList()
                })
                .ToListAsync();

            return studentsWithCourses;
        }
        public async Task<int> CountInstructorQuizzesAsync(string userId)
        {
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (instructor == null)
                return 0;

            int instructorId = instructor.Id;

            
            var quizCount = await _context.Quizzes
                .CountAsync(q => _context.Courses
                    .Any(c => c.Id == q.CourseId && c.InstructorId == instructorId));

            return quizCount;
        }
        
       public async Task<int> CountInstructorCoursesAync(string userId)
        {
            var instructor = await _context.Instructors
              .FirstOrDefaultAsync(i => i.UserId == userId);

            if (instructor == null)
                return 0;

            int instructorId = instructor.Id;
            var CountCourses = await _context.Courses.CountAsync(c => c.InstructorId == instructorId);
            return CountCourses;
        }
        public async Task<int> CountInstructorStudentsAsync(string userId)
        {
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (instructor == null)
                return 0;

            int instructorId = instructor.Id;

           
            var courseIds = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => c.Id)
                .ToListAsync();

            if (!courseIds.Any())
                return 0;

           
            var studentCount = await _context.CourseEnrollment
                .Where(e => courseIds.Contains(e.CourseId))
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync();

            return studentCount;
        }

    }

}