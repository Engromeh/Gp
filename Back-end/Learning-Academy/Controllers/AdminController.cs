using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
          private readonly IAdminRepository _adminRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepostory _instructorRepostory;
        private readonly IStudentRepository _studentRepository;

          public AdminController(IAdminRepository adminRepository,ICourseRepository courseRepository,
              IStudentRepository studentRepository,IInstructorRepostory instructorRepostory)
          {
            _adminRepository = adminRepository;
            _courseRepository = courseRepository;
            _instructorRepostory = instructorRepostory;
            _studentRepository = studentRepository;
            
          }

          [HttpGet]
            public ActionResult<IEnumerable<Admin>> GetAdmins()
            {
                var admins = _adminRepository.GetAllAdmins();
                return Ok(admins);
            }

         [HttpGet("{id}")]
            public IActionResult GetByAdminId(int id)
            {
                 var admin = _adminRepository.GetByAdminId(id);

                 if (admin == null)
                 {
                   return NotFound();
                 }
                 return Ok(admin);
            }
        [HttpPost]
            public IActionResult AddAdmin([FromBody] AdminDto adminDto)
            {
                if (adminDto == null)
                {
                    return BadRequest("Admindata is required.");
                }

                var admin = new Admin
                {
                    UserName=adminDto.UserName,
                    Email = adminDto.Email
                    
                };

                _adminRepository.AddAdmin(admin);

                return CreatedAtAction(nameof(GetByAdminId), new { id = admin.Id }, admin);
            }

        [HttpPut("{id}")]
        public IActionResult UpdateAdmin(int id, [FromBody] AdminDto adminDto)
        {
            if (adminDto == null) return BadRequest();

            var existingAdmin = _adminRepository.GetByAdminId(id);
            if (existingAdmin == null) return NotFound();

            existingAdmin.UserName = adminDto.UserName;
            existingAdmin.Email = adminDto.Email;
            

            _adminRepository.UpdateAdmin(existingAdmin);

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAdmin(int id)
        {
            var existingAdmin = _adminRepository.GetByAdminId(id);
            if (existingAdmin == null) return NotFound();

            _adminRepository.DeleteAdmin(id);
            return Ok($"Admin with ID {id} has been deleted successfully.");
        }
        [HttpGet("courses")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AdminCourseDto>>> GetAllCoursesForAdmin()
        {
            var courses = await _courseRepository.GetAllCoursesForAdminAsync();
            return Ok(courses);
        }
        [HttpGet("courses/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminCourseDto>> GetCourseForAdminById(int id)
        {
            var course = await _courseRepository.GetCourseForAdminByIdAsync(id);

            if (course == null)
                return NotFound();

            return Ok(course);
        }
        [HttpPut("course/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] AdminEditCourseDto updatedCourse)
        {
            var isUpdated = await _courseRepository.UpdateCourseAsync(id, updatedCourse);

            if (!isUpdated)
                return NotFound(new { Message = "Course updated failed." });

            return Ok(new { Message = "Course updated successfully." });
        }

        [HttpDelete("course/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourseById(int id)
        {
            var isDeleted = await _courseRepository.DeleteCourseByIdAsync(id);

            if (!isDeleted)
                return NotFound(new { Message = "Course not found or could not be deleted." });

            return Ok(new { Message = "Course deleted successfully." });
        }
        [HttpGet("Students")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<StudentDto>> GetStudents()
        {

            return await _studentRepository.GetAllStudentsAsync(); ;
        }
        [HttpGet("student/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var stu = await _studentRepository.GetByStudentId(id);

            if (stu == null)
            {
                return NotFound();
            }

            return Ok(stu);
        }
        [HttpGet("instructors")]
        public ActionResult<IEnumerable<Instructor>> GetInstructors()
        {
            var instructors = _instructorRepostory.GetAllInstructors();
            return Ok(instructors);
        }
        [HttpGet("instructor/{id}")]
        public ActionResult GetInstructor(int id)
        {
            var inst = _instructorRepostory.GetByInstructorId(id);
            if (inst == null)
            {
                return NotFound();
            }
            return Ok(inst);
        }




    }
}
