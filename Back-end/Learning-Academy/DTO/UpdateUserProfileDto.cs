﻿namespace Learning_Academy.DTO
{
    public class UpdateUserProfileDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }  
        public string? phone { get; set; }
        public IFormFile? ProfileImage { get; set; }  
        
    }
}
