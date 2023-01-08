using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace OnlineShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }

    }
}
