using System.ComponentModel.DataAnnotations;
using System;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        
        public string Country { get; set; }
        
        
        public string Name { get; set; }
        
        
        
        
        
        

        [Required]
        [StringLength(8, MinimumLength=4)]
        public string Password { get; set; }
        
    }
}

