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

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.CourseEnrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }
        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            return await _context.CourseEnrollment
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            return await _context.CourseEnrollment
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            return await _context.CourseEnrollment
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }
        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            _context.CourseEnrollment.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            _context.Entry(enrollment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.CourseEnrollment.FindAsync(id);
            if (enrollment != null)
            {
                _context.CourseEnrollment.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EnrollmentExistsAsync(int studentId, int courseId)
        {
            return await _context.CourseEnrollment
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
