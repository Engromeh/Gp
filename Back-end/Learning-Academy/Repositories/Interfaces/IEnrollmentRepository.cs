using Learning_Academy.Models;

namespace Learning_Academy.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        IEnumerable<StudentEnrollmentCourse> GetAllEnrollment();

        StudentEnrollmentCourse GetEnrollmentById(int studentId, int courseId);

        void AddEnrollment(StudentEnrollmentCourse studentEnrollmentCourse);
        void UpdateEnrollment(StudentEnrollmentCourse studentEnrollmentCourse);
        void DeleteEnrollment(int studentId, int courseId);
    }
}
