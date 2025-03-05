using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
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
            if(stu== null)
            {
                return NotFound();
            }
            return Ok(stu);
        }
        [HttpPost]
        public ActionResult AddStudent(Student student) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _studentRepository.AddStudent(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateStudent(int id,Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stud = _studentRepository.GetByStudentId(id);
            if (stud == null)
            {
                return NotFound("student not found.");
            }
            else
            {
                stud.FirstName = student.FirstName;
                stud.LastName = student.LastName;
                stud.Email = student.Email;
                _studentRepository.UpdateStudent(stud);
                return NoContent();

            }
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

    }
}
