using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IStudentRepository studentRepository;
        private readonly IInstructorRepostory instructorRepostory;
        private readonly IAdminRepository adminRepository;
        private readonly LearningAcademyContext _context;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration
            , IStudentRepository studentRepository, IInstructorRepostory instructorRepostory, IAdminRepository adminRepository, LearningAcademyContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
            this.studentRepository = studentRepository;
            this.instructorRepostory = instructorRepostory;
            this.adminRepository = adminRepository;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Registration(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
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
                user.UserName = registerDto.UserName;
                user.Email = registerDto.Email;
                user.PhoneNumber = registerDto.Phone;
                user.UserRole = registerDto.Role;
                IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
                IdentityResult role = await _userManager.AddToRoleAsync(user, registerDto.Role);
                if (registerDto.Role == "Student")
                {
                    var student = new Student()
                    {
                        userName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id
                    };
                    // studentRepository.AddStudent(student);
                    _context.Students.Add(student);
                    _context.SaveChanges();
                }
                if (registerDto.Role == "Instructor")
                {
                    var instructor = new Instructor()
                    {
                        userName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id

                    };
                    // instructorRepostory.AddInstructor(instructor);
                    _context.Instructors.Add(instructor);
                    _context.SaveChanges();
                }
                if (registerDto.Role == "Admin")
                {
                    var admin = new Admin()
                    {
                        userName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id
                    };
                    //  adminRepository.AddAdmin(admin);
                    _context.Admins.Add(admin);
                    _context.SaveChanges();
                }
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                if (!role.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok($"user is registered sucessfully as {user.UserRole}");

            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid) { 
                User user=await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null) {
                    bool found =await _userManager.CheckPasswordAsync(user, loginDto.Password);

                    if (found)
                    {
                        //get data in token (cliam)
                         var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.Email,user.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        //get role 
                        var roles=await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
                        SigningCredentials sign = new SigningCredentials( securityKey, SecurityAlgorithms.HmacSha256);


;                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _config["JWT:ValidIssuer"],
                            audience: _config["JWT:ValidAudience"],
                            claims:claims,
                            expires : DateTime.Now.AddDays(1),
                            signingCredentials :sign
                            );
                        return Ok(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration=token.ValidTo

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
