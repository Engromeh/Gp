using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Learning_Academy.Repositories.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Academy.DTO;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {   
        private readonly IInstructorRepostory _instructorRepostory;
        private readonly IChatRepository _chatRepository;
        
        private readonly UserManager<User> _userManager;
        public InstructorController (IInstructorRepostory instructor, IChatRepository chatRepository, UserManager<User> userManager)
        {
            _instructorRepostory = instructor;
            _chatRepository = chatRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable< Instructor>> GetInstructors() {
           var instructors = _instructorRepostory.GetAllInstructors();
            return Ok(instructors);
        }
        [HttpGet("{id}")]
        public ActionResult GetInstructor(int id)
        {
            var inst=_instructorRepostory.GetByInstructorId(id);
            if(inst == null) {
                return NotFound();
            }
            return Ok(inst);
        }

        [HttpPost]
        public ActionResult AddInstructor(InstructorDto instructorDto)
        {
          
           if (instructorDto == null)
            {
                return BadRequest("required adding data");
            }
            var instruct = new Instructor
            {
               
                UserName = instructorDto.UserName,
                Email = instructorDto.Email

            };
            _instructorRepostory.AddInstructor(instruct);
            return CreatedAtAction(nameof(GetInstructor), new { id = instruct.Id }, instruct);

        }
       [HttpPut("{id}")]
       public ActionResult UpdateInstructor(int id,[FromBody]InstructorDto instructor ) {
            var instruct = _instructorRepostory.GetByInstructorId(id);
            if(instruct == null)
            {
                return BadRequest("instructor not found ");
            }
            
            instruct.UserName = instructor.UserName;
            instruct.Email = instructor.Email;
            _instructorRepostory.UpdateInstructor(instruct);
            return Ok("intructor is updated");

           

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _instructorRepostory.GetInstructorByIdAsync(id);
            if (instructor == null)
            {
                return NotFound(new { Message = "Instructor not found." });
            }

            await _instructorRepostory.DeleteInstructorAsync(id);

            return Ok("Instructor id deleted");
        }
        [Authorize(Roles ="Instructor")]
        [HttpGet("students-with-courses")]
        public async Task<IActionResult> GetStudentsWithCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _instructorRepostory.GetStudentsWithTheirCoursesAsync(userId);
            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("my/quizzes/count")]
        public async Task<ActionResult<int>> CountMyQuizzes()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            var quizCount = await _instructorRepostory.CountInstructorQuizzesAsync(userId);

            return Ok(quizCount);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("my/Courses/count")]
        public async Task<ActionResult<int>> CountMyCourses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            var Courses = await _instructorRepostory.CountInstructorCoursesAync(userId);

            return Ok(Courses);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("my/Students/count")]
        public async Task<ActionResult<int>> CountMyStudents()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            string userId = userIdClaim.Value;

            var Count = await _instructorRepostory.CountInstructorStudentsAsync(userId);

            return Ok(Count);
        }


    }
}
