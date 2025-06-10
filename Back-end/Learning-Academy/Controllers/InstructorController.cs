using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Learning_Academy.Repositories.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Academy.DTO;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;

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
               
                UserName = instructorDto.LastName,
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
            
            instruct.UserName = instructor.LastName;
            instruct.Email = instructor.Email;
            _instructorRepostory.UpdateInstructor(instruct);
            return Ok("intructor is updated");

           

        }

       [HttpDelete("{id}")]
       public ActionResult DeleteInstructor (int id)
        {
            var instr = _instructorRepostory.GetByInstructorId(id);
            if (instr == null)
            {
                return NotFound("instructor not found.");
            }
            else
            {
                _instructorRepostory.DeleteInstructor(id);
                return Ok($"this instructor iD {id} is deleted ");
            }
        }
        
    }
}
