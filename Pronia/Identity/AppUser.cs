using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Pronia.Identity
{
    public class AppUser:IdentityUser
    {
        [MaxLength(100)]
        public string fullname { get; set; }
        public bool IsActive { get; set; }
    }
}
