using Learning_Academy.DTO;
using Learning_Academy.Models;
using Learning_Academy.Repositories.Classes;
using Learning_Academy.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Data;

namespace Learning_Academy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<User> signIn;
        private readonly LearningAcademyContext _context;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration
            , LearningAcademyContext context, SignInManager<User> signIn)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
            _context = context;
            this.signIn = signIn;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Registration(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            User user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone,
                UserRole = registerDto.Role
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            // Use async save
            if (registerDto.Role == "Student")
            {
                var student = new Student
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserId = user.Id
                };

                await _context.Students.AddAsync(student);
            }
            else if (registerDto.Role == "Instructor")
            {
                var instructor = new Instructor
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserId = user.Id
                };

                await _context.Instructors.AddAsync(instructor);
            }
            else if (registerDto.Role == "Admin")
            {
                var admin = new Admin
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserId = user.Id
                };

                await _context.Admins.AddAsync(admin);
            }

            await _context.SaveChangesAsync();

            return Ok($"User is registered successfully as {user.UserRole}");
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
       
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signIn.SignOutAsync(); 
            return Ok("Logged out successfully."); 
        }

        [HttpGet("google-signup")]
        public IActionResult GoogleSignUp(string role)
        { 

            var redirectUrl = Url.Action("GoogleSignUpCallback", "User", new { role = role });
            var properties = signIn.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet("google-signin")]
        public IActionResult GoogleSignIn()
        {
            var redirectUrl = Url.Action("GoogleSignInCallback", "User");
            var properties = signIn.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet("google-signup-callback")]
        public async Task<IActionResult> GoogleSignUpCallback(string role)
        {
            var info = await signIn.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("Error: Could not retrieve Google login info.");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return BadRequest("User already exists. Please sign in instead.");

            var user = new User { UserName = email, Email = email, UserRole = role };

           
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

           
            if (!string.IsNullOrEmpty(role))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!addToRoleResult.Succeeded)
                    return BadRequest("Failed to assign role to user.");
            }

            await _userManager.AddLoginAsync(user, info);

            
            if (role == "Student")
            {
                var student = new Student { UserId = user.Id, Email=email , UserName = email };
                _context.Students.Add(student);
            }
            else if (role == "Instructor")
            {
                var instructor = new Instructor { UserId = user.Id ,Email=email,UserName =email };
                _context.Instructors.Add(instructor);
            }
            else if (role == "Admin")
            {
                var admin = new Admin { UserId = user.Id ,Email= email , UserName = email };
                _context.Admins.Add(admin);
            }

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return Ok(new { token, email = user.Email, message = "Signed up successfully with Google." });
        }


        [HttpGet("google-signin-callback")]
        public async Task<IActionResult> GoogleSignInCallback()
        {
            var info = await signIn.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("Error: Could not retrieve Google login info.");

            var result = await signIn.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (!result.Succeeded)
                return BadRequest("Invalid login. Please sign up first.");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            var token = GenerateJwtToken(user);
            return Ok(new { token, email = user.Email, message = "Signed in successfully with Google." });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.UserRole) 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);



            var resetLink = $"https://localhost:5241/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";


           
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderName = _config["EmailSettings:SenderName"];
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];

            var client = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "Reset Your Password",
                Body = $"Click to reset: <a href='{resetLink}'>Reset Password</a>",
                IsBodyHtml = true
            };

            message.To.Add(user.Email);
            await client.SendMailAsync(message);

            return Ok("Reset link has been sent.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");

            var decodedToken = WebUtility.UrlDecode(model.Token);

    var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password reset successfully.");
        }





    }
}
