using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {


        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            var courses = _courseRepository.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetByCourseId(int id)
        {
            var course = _courseRepository.GetByCourseId(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        ///  Search function  in home page 
        [HttpPost("{name}")]
        public ActionResult<Course> Search(string name)
        {
            var course =_courseRepository.GetByCourseName(name);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public IActionResult AddCourse([FromBody] CourseDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest("Course data is required.");
            }

            var course = new Course
            {
                CourseName = courseDto.CourseName,
                CourseDescription = courseDto.CourseDescription,
                AdminId = courseDto.AdminId,
                InstructorId = courseDto.InstructorId,
                CertificateId = courseDto.CertificateId
            };

            _courseRepository.AddCourse(course);

            return CreatedAtAction(nameof(GetByCourseId), new { id = course.Id }, course);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, CourseDto courseDto)
        {
            var entity = _courseRepository.GetByCourseId(id);
            if (entity == null)
            {
                return NotFound("Course not found");
            }

            entity.CourseName = courseDto.CourseName;
            entity.CourseDescription = courseDto.CourseDescription;
            entity.AdminId = courseDto.AdminId;
            entity.InstructorId = courseDto.InstructorId;
            entity.CertificateId = courseDto.CertificateId;

            _courseRepository.UpdateCourse(entity);

            return Ok("Course updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var existingCourse = _courseRepository.GetByCourseId(id);

            if (existingCourse == null)
            {
                return NotFound("Course not found.");
            }
            else
            {

                _courseRepository.DeleteCourse(id);

                return Ok($"Course with ID {id} has been deleted successfully.");
            }
        }






    }
}

