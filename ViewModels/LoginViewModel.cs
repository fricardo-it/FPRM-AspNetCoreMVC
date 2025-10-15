using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}