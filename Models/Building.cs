using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPRMAspNetCoreMVC.Models;

public class Building : Entity
{
    [Display(Name = "Manager")]
    public Guid? ManagerId { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(200, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(1000, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 0)]
    public string Description { get; set; }

    [Display(Name = "Construction Date")]
    [DataType(DataType.Date)]
    public DateTime ConstructionDate { get; set; }

    [Display(Name = "Registration Date")]
    [DataType(DataType.Date)]
    public DateTime RegistrationDate { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(200, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
    public string Street { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(100, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
    public string City { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(2, ErrorMessage = "Field {0} must be {2} chars", MinimumLength = 2)]
    public string Province { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(6, ErrorMessage = "Field {0} must be {2} chars. Format = N0N0N0")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(200, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 2)]
    public string Country { get; set; }

    [DisplayName("Active?")]
    public bool IsActive { get; set; }

    [DisplayName("Image")]
    public byte[]? ImageData { get; set; }
    

    /* EF Relation */
    public ICollection<Apartment>? Apartments { get; set; }
    public User Manager { get; set; }
}