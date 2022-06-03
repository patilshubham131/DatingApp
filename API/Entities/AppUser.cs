using System;
using System.Collections.Generic;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        //commnetion these 4 props because we are inheriting from identity user.
        // public int Id { get; set; }
        // public string UserName { get; set; }
        
        // public byte[] PasswordHash { get; set; }

        // public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActivated { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public ICollection<Photo> Photos {get; set;}

        public ICollection<UserLike> LikedByUsers { get; set; }

        public ICollection<UserLike> LikedUser { get; set; }
        public ICollection<Message> MessageSent { get; set; }
        
        public ICollection<Message> MessageReceived { get; set; }
        
        
        public ICollection<AppUserRole> UserRoles{get; set;}

        // public int GetAge(){
        //     return DateOfBirth.CalculateAge();
        // }
        
    }
}