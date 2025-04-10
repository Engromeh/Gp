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
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentController(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Attachment>> GetAttachment()
        {
            var Attachment = _attachmentRepository.GetAllAttach();
            return Ok(Attachment);
        }

        [HttpGet("{id}")]
        public IActionResult GetAttachById(int id)
        {
            var Attachment = _attachmentRepository.GetAttachById(id);

            if (Attachment == null)
            {
                return NotFound();
            }
            return Ok(Attachment);
        }
        [HttpPost]
        public IActionResult AddAttach([FromBody] AttachmentDto attachmentDto)
        {
            if (attachmentDto == null)
            {
                return BadRequest("Attachment data is required.");
            }

            var attach = new Attachment
            {
                Name = attachmentDto.Name,
                Type = attachmentDto.Type,
                Size = attachmentDto.Size,
                MassageId=attachmentDto.MassegeId
            };

            _attachmentRepository.AddAttach(attach);
            

            return CreatedAtAction(nameof(GetAttachment), new { id = attach.Id }, attach);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAttach(int id, [FromBody] AttachmentDto attachmentDto)
        {
            if (attachmentDto == null) return BadRequest();

            var existingAttach = _attachmentRepository.GetAttachById(id);
            if (existingAttach == null) return NotFound();

            existingAttach.Name = attachmentDto.Name;
             existingAttach.Type = attachmentDto.Type;
             existingAttach.Size = attachmentDto.Size;
             existingAttach.MassageId = attachmentDto.MassegeId;
            

            _attachmentRepository.UpdateAttach(existingAttach);
            
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAttach(int id)
        {
            var existingAttach = _attachmentRepository.GetAttachById(id);
            if (existingAttach == null) return NotFound();

            _attachmentRepository.DeleteAttach(id);
            
            return Ok($"Attachment with ID {id} has been deleted successfully.");
        }
    }
}
