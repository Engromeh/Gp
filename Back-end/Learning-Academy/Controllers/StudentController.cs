using Learning_Academy.Models;
using Learning_Academy.DTO;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {   
        private readonly IStudentRepository _studentRepository;
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<User> _userManager;
        private readonly LearningAcademyContext _context;
        public StudentController(IStudentRepository studentRepository,LearningAcademyContext learningAcademyContext, IChatRepository chatRepository, UserManager<User> userManager)
        {
            _studentRepository = studentRepository;
            _chatRepository = chatRepository;
            _userManager = userManager;
            _context = learningAcademyContext;
        }
        [HttpGet]

        public async Task<IEnumerable<StudentDto>> GetStudents()
        {
          
            return await _studentRepository.GetAllStudentsAsync(); ;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var stu = await _studentRepository.GetByStudentId(id);

            if (stu == null)
            {
                return NotFound();
            }

            return Ok(stu); 
        }


        [HttpPost]
        public ActionResult AddStudent([FromBody]StudentDto studentDto) {
           if(studentDto == null)
            {
                return BadRequest("data is required");
            }
            var stud = new Student()
            {

                UserName = studentDto.UserName,
                Email = studentDto.Email
                
                
            };
            _studentRepository.AddStudent(stud);
            return CreatedAtAction(nameof(GetStudent), new { id = stud.Id }, stud);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateStudent(int id,StudentDto student)
        {
           var stud=_studentRepository.GetByStudentId(id);
            if(stud== null)
            {
                return BadRequest("not found");
            }
           // stud.UserName =student.UserName;
          
          //  stud.Email=student.Email;

            _studentRepository.UpdateStudent(stud);
            return Ok(stud);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            var stud = _studentRepository.GetByStudentId(id);
            if (stud == null)
            {
                return NotFound("student not found.");
            }
            else
            {
                _studentRepository.DeleteStudent(id);
                return Ok($"this student iD {id} is deleted ");
            }
        }
        //GetInterests
        [Authorize(Roles = "Student")]
        [HttpGet("interests")]
        public async Task<IActionResult> GetMyInterests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var student = await _context.Students
                .Include(s => s.Interests)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return NotFound("Student not found.");

            var interests = student.Interests.Select(i => i.Category).ToList();
            return Ok(interests);
        }
        //Add Interest 
        [Authorize(Roles = "Student")]
        [HttpPost("interests/add")]
        public async Task<IActionResult> AddInterest([FromBody] string category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var student = await _context.Students
                .Include(s => s.Interests)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound("Student not found.");

            if (student.Interests.Any(i => i.Category.ToLower() == category.ToLower()))
                return BadRequest("❌ Interest already exists.");

            _context.StudentInterests.Add(new StudentInterest
            {
                StudentId = student.Id,
                Category = category.Trim()
            });

            await _context.SaveChangesAsync();
            return Ok("✅ Interest added successfully.");
        }
        //Update Interests
        [Authorize(Roles = "Student")]
        [HttpPut("interests/update")]
        public async Task<IActionResult> UpdateInterests([FromBody] StudentInterestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var student = await _context.Students
                .Include(s => s.Interests)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound("Student not found.");

            _context.StudentInterests.RemoveRange(student.Interests);

            foreach (var category in dto.Categories.Distinct())
            {
                _context.StudentInterests.Add(new StudentInterest
                {
                    StudentId = student.Id,
                    Category = category.Trim()
                });
            }

            await _context.SaveChangesAsync();
            return Ok("✅ Interests updated successfully.");
        }
        //DeleteInterest
        [Authorize(Roles = "Student")]
        [HttpDelete("interests/{category}")]
        public async Task<IActionResult> DeleteInterest(string category)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var student = await _context.Students
                .Include(s => s.Interests)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return NotFound("Student not found.");

            var interestToRemove = student.Interests.FirstOrDefault(i => i.Category.ToLower() == category.ToLower());
            if (interestToRemove == null)
                return NotFound("Interest not found.");

            _context.StudentInterests.Remove(interestToRemove);
            await _context.SaveChangesAsync();

            return Ok($"✅ Interest '{category}' has been deleted.");
        }

    }
}
