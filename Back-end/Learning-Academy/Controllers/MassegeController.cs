using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MassegeController : ControllerBase
    {
        private readonly IMassageRepository _massageRepository;
        public MassegeController(IMassageRepository massageRepository)
        {
            _massageRepository = massageRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Massage>> GetAllMassege()
        {
            var massege= _massageRepository.GetAllMassage();
            return Ok(massege);
        }
        [HttpGet("{id}")]
        public IActionResult GetMassegeById(int id)
        {
            var message = _massageRepository.GetMassageById(id);
            if(message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }
        [HttpPost]
        public IActionResult AddMassege([FromBody] MassegeDto massegeDto)
        {
            if (massegeDto == null)
            {
                return BadRequest("Massege data is required.");
            }

            var massege = new Massage
            {
                Content = massegeDto.Content,
                StudentId = massegeDto.StudentId,
                InstructorId = massegeDto.InstructorId,
                

            };

            _massageRepository.AddMassage(massege);

            return CreatedAtAction(nameof(GetMassegeById), new { id = massege.Id },massege);
        }
        [HttpPut("{Id}")]
        public IActionResult UpdateMassege(int Id, [FromBody] MassegeDto massegeDto)
        {
            if (massegeDto == null) return BadRequest();

            var existingMassege = _massageRepository.GetMassageById(Id);
            if (existingMassege == null) return NotFound();

            existingMassege.Content = massegeDto.Content;
            existingMassege.StudentId = massegeDto.StudentId;
            existingMassege.InstructorId = massegeDto.InstructorId;

            _massageRepository.UpdateMassage(existingMassege);
          
            return Ok("Massege updated");
        }
        [HttpDelete]
        public IActionResult DeleteMassege(int id)
        {
            var massege = _massageRepository.GetMassageById( id);
            if (massege == null) return NotFound();

            _massageRepository.DeleteMassage(id);
            return Ok($"Massege with ID {id} has been deleted successfully.");
        }
    }
}
