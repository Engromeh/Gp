using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(inst);
        }
        
    }
}
