    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    namespace WebApplication1.Models
    {
        public class User : IdentityUser
        {
        [Key]
            public int UserId { get; set; }
            public override string? UserName { get; set; }
            public override string? Email { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }

        }
    }
