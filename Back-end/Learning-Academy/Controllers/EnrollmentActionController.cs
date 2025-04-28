using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
    [Authorize(Roles = "Instructor")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentActionController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
       


        public EnrollmentActionController(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
           
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
           
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveEnrollment(int id)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            // Check if current user is the instructor for this course
            if (!await IsCourseInstructor(enrollment.CourseId))
            {
                return Forbid();
            }

            enrollment.Status = "Approved";
            await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);

            return NoContent();
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectEnrollment(int id)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            // Check if current user is the instructor for this course
            if (!await IsCourseInstructor(enrollment.CourseId))
            {
                return Forbid();
            }

            //Prevent rejecting if already approved
             if (enrollment.Status == "Approved")
             {
                return BadRequest("You cannot reject an enrollment that has already been approved.");
             }

            enrollment.Status = "Rejected";
            await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);

            return NoContent();
        }

        private EnrollmentResponseDto MapToEnrollmentResponseDto(Enrollment enrollment)
        {
            return new EnrollmentResponseDto
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.UserName,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course?.CourseName,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status,
                CourseInstructorId = enrollment.Course?.Instructor?.Id ?? 0,
                CourseInstructorName = enrollment.Course?.Instructor?.UserName
            };
        }
        private async Task<bool> IsCourseInstructor(int courseId)
        {
            // Get user ID from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Get course with its instructor
            var course = await _courseRepository.GetByIdWithInstructorAsync(courseId);
            if (course?.Instructor == null)
            {
                return false;
            }

            // Compare user ID directly
            return course.Instructor.UserId == userId;
        }
        
           
    }
}
