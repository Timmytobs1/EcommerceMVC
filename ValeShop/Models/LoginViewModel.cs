using System.ComponentModel.DataAnnotations;

namespace ValeShop.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
