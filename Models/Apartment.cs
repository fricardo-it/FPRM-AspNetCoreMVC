using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.Models;

public class Apartment : Entity
{
    [DisplayName("Building")]
    public Guid BuildingId { get; set; }

    [DisplayName("Manager")]
    public Guid ManagerId { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(20, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 1)]
    public string Unit { get; set; }

    [DisplayName("Type")]
    public string TypeApartment { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(1000, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 0)]
    public string Description { get; set; }


    [Required(ErrorMessage = "Field {0} is required")]
    [Range(0, 10, ErrorMessage = "Field {0} must be between {1} and {2}")]
    public int Bathrooms { get; set; }

    [DisplayName("Area (m²)")]
    public double Area { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Field {0} must be greater than or equal to {1}")]
    [DisplayName("Monthly Rent")]
    public decimal RentAmount { get; set; }

    [DisplayName("Registration Date")]
    [DataType(DataType.Date)]
    public DateTime RegistrationDate { get; set; }

    [DisplayName("Available?")]
    public bool IsAvailable { get; set; }

    [DisplayName("Image")]
    public byte[] ImageData { get; set; }

    /* EF Relation */
    public Building Building { get; set; }
}