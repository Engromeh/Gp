using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Models
{
    public class LearningAcademyContext :DbContext
    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet <Course> Courses { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Massage> Massages { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<StudentEnrollmentCourse> StudentEnrollmentCourse { get; set; }
        public virtual DbSet<StudentRatingCourse> StudentRatingCourse { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-LSMDLDO\\SQLEXPRESS;Database=LearningAcademy;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True", sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
        maxRetryCount: 3,  // Increase max retries
        maxRetryDelay: TimeSpan.FromSeconds(15),  // Increase delay
        errorNumbersToAdd: null));
        }
    }


 }

