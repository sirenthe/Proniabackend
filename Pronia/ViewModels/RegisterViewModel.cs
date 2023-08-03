using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Fullname { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
