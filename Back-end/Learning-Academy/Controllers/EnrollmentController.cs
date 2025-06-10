using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
          private readonly ICourseRepository _courseRepository;
        private readonly LearningAcademyContext _context;
       

        public EnrollmentController(LearningAcademyContext learningAcademy, IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            _context = learningAcademy;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }
       

        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentEnrollments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            if (_context == null || _context.Students == null)
                return StatusCode(500, "Database context or Students DbSet is not initialized.");

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return NotFound("Student not found.");

            var enrollments = await _context.CourseEnrollment
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .ThenInclude(c => c.Instructor)
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.CourseName,
                    CourseDescription = e.Course.CourseDescription,
                    CourseRating = e.Course != null && e.Course.CourseRatinds.Any() ? (int)e.Course.CourseRatinds.Average(cr => cr.RatingValue) : 0,
                    CourseInstructorName = e.Course != null && e.Course.Instructor != null ? e.Course.Instructor.UserName : "N/A"
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollments()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            var enrollmentDtos = enrollments.Select(e => MapToEnrollmentResponseDto(e));
            return Ok(enrollmentDtos);
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return MapToEnrollmentResponseDto(enrollment);
        }

        // GET: api/Enrollments/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollmentsByStudent(int studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId);
            var enrollmentDtos = enrollments.Select(e => MapToEnrollmentResponseDto(e));
            return Ok(enrollmentDtos);
        }

        // GET: api/Enrollments/course/5
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollmentsByCourse(int courseId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
            var enrollmentDtos = enrollments.Select(e => MapToEnrollmentResponseDto(e));
            return Ok(enrollmentDtos);
        }

        // POST: api/Enrollments
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> PostEnrollment(CreateEnrollmentDto createEnrollmentDto)
        {
            // Check if enrollment already exists
            if (await _enrollmentRepository.EnrollmentExistsAsync(createEnrollmentDto.StudentId, createEnrollmentDto.CourseId))
            {
                return Conflict("Student is already enrolled in this course.");
            }

            var enrollment = new Enrollment
            {
                StudentId = createEnrollmentDto.StudentId,
                CourseId = createEnrollmentDto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await _enrollmentRepository.AddEnrollmentAsync(enrollment);

            // Reload the enrollment with related data
            var createdEnrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollment.Id);
            var enrollmentResponse = MapToEnrollmentResponseDto(createdEnrollment);

            return CreatedAtAction("GetEnrollment", new { id = enrollment.Id }, enrollmentResponse);
        }


        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            await _enrollmentRepository.DeleteEnrollmentAsync(id);

            return NoContent();
        }

        // Helper method to map Enrollment to EnrollmentResponseDto
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
                Status = enrollment.Status
            };
        }






    }
}

