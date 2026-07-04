using System.ComponentModel.DataAnnotations;

namespace MXC.Models.Login
{
    public sealed class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PassWord { get; set; }
    }
}
