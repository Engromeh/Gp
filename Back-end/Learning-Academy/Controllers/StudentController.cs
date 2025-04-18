﻿using Learning_Academy.Models;
using Learning_Academy.DTO;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Learning_Academy.Controllers
{
    // [Authorize]
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
        public ActionResult AddStudent([FromBody]StudentDto studentDto) {
           if(studentDto == null)
            {
                return BadRequest("data is required");
            }
            var stud = new Student()
            {

                userName=studentDto.userName,
                Email = studentDto.Email,
                AdminId=studentDto.AdminId,
                
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
            stud.userName = student.userName;
            stud.Email=student.Email;

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

    }
}
