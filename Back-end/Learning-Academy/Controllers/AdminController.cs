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
    public class AdminController : ControllerBase
    {
          private readonly IAdminRepository _adminRepository;

          public AdminController(IAdminRepository adminRepository)
          {
            _adminRepository = adminRepository;
          }

          [HttpGet]
            public ActionResult<IEnumerable<Admin>> GetAdmins()
            {
                var admins = _adminRepository.GetAllAdmins();
                return Ok(admins);
            }

         [HttpGet("{id}")]
            public IActionResult GetByAdminId(int id)
            {
                 var admin = _adminRepository.GetByAdminId(id);

                 if (admin == null)
                 {
                   return NotFound();
                 }
                 return Ok(admin);
            }
        [HttpPost]
            public IActionResult AddAdmin([FromBody] AdminDto adminDto)
            {
                if (adminDto == null)
                {
                    return BadRequest("Admindata is required.");
                }

                var admin = new Admin
                {
                    UserName=adminDto.UserName,
                    Email = adminDto.Email
                    
                };

                _adminRepository.AddAdmin(admin);

                return CreatedAtAction(nameof(GetByAdminId), new { id = admin.Id }, admin);
            }

        [HttpPut("{id}")]
        public IActionResult UpdateAdmin(int id, [FromBody] AdminDto adminDto)
        {
            if (adminDto == null) return BadRequest();

            var existingAdmin = _adminRepository.GetByAdminId(id);
            if (existingAdmin == null) return NotFound();

            existingAdmin.UserName = adminDto.UserName;
            existingAdmin.Email = adminDto.Email;
            

            _adminRepository.UpdateAdmin(existingAdmin);

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAdmin(int id)
        {
            var existingAdmin = _adminRepository.GetByAdminId(id);
            if (existingAdmin == null) return NotFound();

            _adminRepository.DeleteAdmin(id);
            return Ok($"Admin with ID {id} has been deleted successfully.");
        }
    }
}
