using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpGet]
        public IActionResult GetAllEnrollment()
        {
            var enrollments = _enrollmentRepository.GetAllEnrollment();
            return Ok(enrollments);
        }

        [HttpGet("{studentId}/{courseId}")]
        public IActionResult GetEnrollmentById(int studentId, int courseId)
        {
            var enrollment = _enrollmentRepository.GetEnrollmentById(studentId, courseId);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        [HttpPost]
        public IActionResult AddEnrollment([FromBody] EnrollmentDto enrollmentDto)
        {
            if (enrollmentDto == null)
                return BadRequest("Invalid data");

            var enrollment = new StudentEnrollmentCourse
            {
                StudentId = enrollmentDto.StudentId,
                CourseId = enrollmentDto.CourseId,
                
            };

            _enrollmentRepository.AddEnrollment(enrollment);


            return CreatedAtAction(nameof(GetEnrollmentById), new { studentId = enrollment.StudentId, courseId = enrollment.CourseId }, enrollment);
        }

       /* [HttpPut("{studentId}/{courseId}")]
        public IActionResult UpdateEnrollment(int studentId, int courseId, [FromBody] EnrollmentDto enrollmentDto)
        {
            if (enrollmentDto == null || studentId != enrollmentDto.StudentId || courseId != enrollmentDto.CourseId)
                return BadRequest("Invalid or mismatched IDs");

            var enrollment = _enrollmentRepository.GetEnrollmentById(studentId, courseId);
            if (enrollment == null)
                return NotFound();

            // Update properties
            enrollment.EnrollmentDate = enrollmentDto.EnrollmentDate;

            _enrollmentRepository.Update(enrollment);
            return NoContent();
        }
       */
        [HttpDelete("{studentId}/{courseId}")]
        public IActionResult DeleteEnrollment(int studentId, int courseId)
        {
            _enrollmentRepository.DeleteEnrollment(studentId, courseId);
            return Ok("Student drop the course");
        }
    }
}

