using Learning_Academy.Models.QuizModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Models
{
    public class LearningAcademyContext : IdentityDbContext<User>
    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<Enrollment> CourseEnrollment { get; set; }
        public virtual DbSet<CourseRating> CourseRatings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server= .;Database=LearningAcademy;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True", sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
        //       maxRetryCount: 3,  // Increase max retries
        //       maxRetryDelay: TimeSpan.FromSeconds(15),  // Increase delay
        //       errorNumbersToAdd: null));

        //}


        public LearningAcademyContext(DbContextOptions<LearningAcademyContext> options)
       : base(options) { }

        
    }



}

