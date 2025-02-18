using Learning_Academy.Models;
using Learning_Academy.Repositories;
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
          public ActionResult<Course> GetCourse(int id)
          {
              var course = _courseRepository.GetByCourseId(id);
              if (course == null)
              {
                  return NotFound();
              }
              return Ok(course);
          }


         
    
    
    }
}

