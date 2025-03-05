using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Learning_Academy.Repositories.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Learning_Academy.DTO;
using NuGet.Protocol.Core.Types;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {   
        private readonly IInstructorRepostory _instructorRepostory;
        public InstructorController (IInstructorRepostory instructor)
        {
            _instructorRepostory = instructor;
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
        public ActionResult AddInstructor(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _instructorRepostory.AddInstructor(instructor);
            return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, instructor);

        }
       [HttpPut("{id}")]
       public ActionResult UpdateInstructor(int id,[FromBody]Instructor instructor ) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingInstructor = _instructorRepostory.GetByInstructorId(id);
            if (existingInstructor == null)
            {
                return NotFound("Instructor not found.");
            }
            else
            {
                existingInstructor.FirstName = instructor.FirstName;
                existingInstructor.LastName = instructor.LastName;
                existingInstructor.Email = instructor.Email;
                _instructorRepostory.UpdateInstructor(existingInstructor);
                return NoContent();

            }

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
