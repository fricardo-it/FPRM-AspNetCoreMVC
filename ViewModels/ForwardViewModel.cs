using System;
using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.Models
{
    public class ForwardViewModel
    {
        [Required]
        public Guid OriginalMessageId { get; set; }

        [Required(ErrorMessage = "Please select an administrator")]
        public string ForwardedToId { get; set; }

        [Required(ErrorMessage = "Please enter message content")]
        public string ForwardedMessageContent { get; set; }


    }
}