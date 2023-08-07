using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace Pronia.ViewModels
{
    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string UsernameorEmail { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
