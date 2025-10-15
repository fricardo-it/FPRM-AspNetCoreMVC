using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FPRMAspNetCoreMVC.Models;

public class User : Entity
{
    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(200, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
    public String Name { get; set; }

    [DisplayName("User Type")]
    public UserRole UserRole { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}