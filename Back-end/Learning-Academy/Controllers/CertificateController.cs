using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateRepository _certificateRepository;

        public CertificateController(ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Certificate>> GetCertificate()
        {
            var certificate = _certificateRepository.GetAllCertificate();
            return Ok(certificate);
        }

        [HttpGet("{id}")]
        public IActionResult GetCertificateById(int id)
        {
            var certificate = _certificateRepository.GetCertificateById(id);

            if (certificate == null)
            {
                return NotFound();
            }
            return Ok(certificate);
        }
        [HttpPost]
        public IActionResult AddCertificate([FromBody] CertificateDto certificateDto)
        {
            if (certificateDto == null)
            {
                return BadRequest("Certificate Data is required.");
            }

            var certificate = new Certificate
            {
                // Id= certificateDto.Id,  //Auto Increment يدويًا وده غلط لو هو Idانا بحدد  Auto Increment.   ، 
                CourseId = certificateDto.CourceId

            };

            _certificateRepository.AddCertificate(certificate);
         
            return CreatedAtAction(nameof(GetCertificateById), new { id = certificate.Id }, certificate);
        }

        [HttpDelete]
        public IActionResult DeleteCertificate(int id)
        {
            var existingCertificate = _certificateRepository.GetCertificateById(id);
            if (existingCertificate == null) return NotFound();

            _certificateRepository.DeleteCertificate(id);

            return Ok($"certificate with ID {id} has been deleted successfully.");
        }
    }
}
