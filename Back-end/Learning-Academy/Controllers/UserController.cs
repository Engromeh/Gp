using Learning_Academy.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Learning_Academy.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Registration(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var validRoles = new List<string> { "Admin", "Student", "Instructor" };
                if (!validRoles.Contains(registerDto.Role))
                {
                    return BadRequest("Invalid role.");
                }
                var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);
                if (!roleExists)
                {
                    return BadRequest("Role does not exist in the database.");
                }
                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone,
                    UserRole = registerDto.Role
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
                IdentityResult role = await _userManager.AddToRoleAsync(user, registerDto.Role);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                //// Assign Role
                //await _userManager.AddToRoleAsync(user, registerDto.Role); // Role = "Instructor", "Student", "Admin"

                //// Create Profile based on Role
                //switch (registerDto.Role)
                //{
                //    case "Instructor":
                //        _context.Instructors.Add(new Instructor { UserId = user.Id });
                //        break;

                //    case "Student":
                //        _context.Students.Add(new Student { UserId = user.Id });
                //        break;

                //    case "Admin":
                //        _context.Admins.Add(new Admin { UserId = user.Id });
                //        break;

                //    default:
                //        return BadRequest("Role not recognized");
                //}

                //await _context.SaveChangesAsync();

                return Ok("user is registered sucessfully");

            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                    if (found)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, user.Id) // ✅ ده الكرت السحري
                        };

                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
                        SigningCredentials sign = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _config["JWT:ValidIssuer"],
                            audience: _config["JWT:ValidAudience"],
                            claims: claims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: sign
                        );

                        return Ok(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            return Unauthorized();
        }

    }
}