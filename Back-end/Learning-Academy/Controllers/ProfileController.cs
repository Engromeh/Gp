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
            return userIdClaim.Value;
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
                Bio = user.Profile?.Bio,
                ProfileImageUrl = user.Profile?.ProfileImageUrl
            };

            return Ok(dto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto model)
        {
            var userId = GetUserId(); // تجيب اليوزر من التوكن

            var user = await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            // تحديث الاسم والإيميل
            user.UserName = model.Username ?? user.UserName;
            user.Email = model.Email ?? user.Email;

            // تحديث الباسورد لو فيه جديد (تأكد تستخدم PasswordHasher)
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
            }

            // تحديث البايو
            if (user.Profile == null)
                user.Profile = new Profile();

            user.Profile.Bio = model.Bio ?? user.Profile.Bio;

            // تحديث صورة البروفايل لو موجودة
            if (model.ProfileImage != null)
            {
                var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.Profile.ProfileImageUrl = $"/images/{fileName}";
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }

    }
}
