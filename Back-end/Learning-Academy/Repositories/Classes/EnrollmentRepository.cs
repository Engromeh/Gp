using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Learning_Academy.Repositories.Classes
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly LearningAcademyContext _context;

        public EnrollmentRepository(LearningAcademyContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentEnrollmentCourse> GetAllEnrollment()
        {
            return _context.StudentEnrollmentCourse.ToList();
        }

        public StudentEnrollmentCourse GetEnrollmentById(int studentId, int courseId)
        {
            return _context.StudentEnrollmentCourse.Find(studentId, courseId);
        }

        public void UpdateEnrollment(StudentEnrollmentCourse studentEnrollmentCourse)
        {
            _context.StudentEnrollmentCourse.Update(studentEnrollmentCourse);
            _context.SaveChanges();
        }
        public void AddEnrollment(StudentEnrollmentCourse studentEnrollmentCourse)
        {
            _context.StudentEnrollmentCourse.Add(studentEnrollmentCourse);
            _context.SaveChanges();
        }

        public void DeleteEnrollment(int studentId, int courseId)
        {
            var enrollment = GetEnrollmentById(studentId, courseId);
            if (enrollment != null)
            {
                _context.StudentEnrollmentCourse.Remove(enrollment);
                _context.SaveChanges();
            }
        }
    }
}
