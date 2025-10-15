using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.Models;

public class Rental : Entity
{
    [DisplayName("Apartment")]
    public Guid ApartmentId { get; set; }
    [DisplayName("Tenant")]
    public Guid TenantId { get; set; }
    [DisplayName("Rent Date")]
    public DateTime InitialRentDate { get; set; }
    [DisplayName("Final Date")]
    public DateTime FinalRentDate { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Field {0} must be greater than or equal to {1}")]
    [DisplayName("Monthly Rent")]
    public decimal MonthlyRent { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(20, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 1)]
    public string Description { get; set; }

    /* EF Relation */
    public Apartment Apartment { get; set; }
    public User Tenant { get; set; }
}
