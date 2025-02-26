using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {   
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            var students =_studentRepository.GetAllStudents();
            return Ok(students);
        }
        [HttpGet("{id}")]
        public ActionResult GetStudent(int id)
        {
            var stu=_studentRepository.GetByStudentId(id);
            return Ok(stu);
        }
    }
}
