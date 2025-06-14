using Learning_Academy.DTO;
using Learning_Academy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learning_Academy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly LearningAcademyContext _context;

        public ProfileController(LearningAcademyContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = GetUserId();
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var dto = new ProfileDto
            {
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                ProfileImageUrl = user.Profile?.ProfileImageUrl
            };

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto model)
        {
            var userId = GetUserId();

            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("User not found");

            user.UserName = model.Username ?? user.UserName;
            user.Email = model.Email ?? user.Email;
            

           
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
            }

          
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                if (user.Profile == null)
                {
                    user.Profile = new Profile { UserId = user.Id };
                    _context.Profiles.Add(user.Profile);
                }

                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

                var sanitizedFileName = Path.GetFileNameWithoutExtension(model.ProfileImage.FileName);
                var extension = Path.GetExtension(model.ProfileImage.FileName);
                var fileName = $"{Guid.NewGuid()}_{sanitizedFileName}{extension}";
                var filePath = Path.Combine(imagesPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.Profile.ProfileImageUrl = $"/images/{fileName}";
            }

           
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student != null)
            {
                student.Email = user.Email;
                
                student.UserName = user.UserName;
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.UserId == userId);
            if (instructor != null)
            {
                instructor.Email = user.Email;
               
                instructor.UserName = user.UserName;
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
            if (admin != null)
            {
                admin.Email = user.Email;
                
                admin.UserName = user.UserName;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }

    }
}