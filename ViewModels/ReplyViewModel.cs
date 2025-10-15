using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.Models
{
    public class ReplyViewModel
    {
        [Required(ErrorMessage = "Please enter a reply")]
        public string ReplyContent { get; set; }
    }
}


