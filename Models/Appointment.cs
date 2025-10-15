using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FPRMAspNetCoreMVC.Models;

public class Appointment : Entity
{

    [DisplayName("Tenant")]
    public Guid TenantId { get; set; }

    [DisplayName("Manager")]
    public Guid ManagerId { get; set; }

    [DisplayName("Building")]
    public Guid BuildingId { get; set; }

    [Display(Name = "Appointment Date")]
    [DataType(DataType.DateTime)]
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Field {0} is required")]
    [StringLength(1000, ErrorMessage = "Field {0} must be between {2} and {1} chars", MinimumLength = 0)]
    public string Message { get; set; }

    [DisplayName("Confirmed?")]
    public bool IsConfirmed { get; set; }

    /* EF Relation */
    public User Tenant { get; set; }
    public Building Building { get; set; }
}