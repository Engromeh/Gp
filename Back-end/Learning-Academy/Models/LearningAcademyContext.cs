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
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Massage> Massages { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<StudentEnrollmentCourse> StudentEnrollmentCourse { get; set; }
        public virtual DbSet<StudentRatingCourse> StudentRatingCourse { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Level> Levels { get; set; }


        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
          optionsBuilder.UseSqlServer("Server=DESKTOP-LSMDLDO\\SQLEXPRESS;Database=LearningAcademy;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True", sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
      maxRetryCount: 3,  // Increase max retries
      maxRetryDelay: TimeSpan.FromSeconds(15),  // Increase delay
      errorNumbersToAdd: null));
      } */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= .;Database=LearningAcademy;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True", sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
               maxRetryCount: 3,  // Increase max retries
               maxRetryDelay: TimeSpan.FromSeconds(15),  // Increase delay
               errorNumbersToAdd: null));
        
        }
        public LearningAcademyContext(DbContextOptions<LearningAcademyContext> options)
       : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقة Course -> Levels
            modelBuilder.Entity<Level>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Levels)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة Level -> Videos
            modelBuilder.Entity<Video>()
                .HasOne(v => v.Level)
                .WithMany(l => l.Videos)
                .HasForeignKey(v => v.LevelId)
                .OnDelete(DeleteBehavior.Cascade);

       

        }

    }



}

