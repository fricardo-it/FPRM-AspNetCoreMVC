using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FPRMAspNetCoreMVC.Models
{
    public class Message : Entity
    {
        [Required]
        [DisplayName("Building")]
        public Guid BuildingId { get; set; }

        [Required]
        [DisplayName("Sender")]
        public Guid SenderMsgId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Title { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;


        // EF Relations
        public User SenderMsg { get; set; }
        public Building Building { get; set; }
        public List<string> Replies { get; set; } = new List<string>();
        public Guid BuildingManagerId { get; set; }
        public Guid LastReplySenderId { get; set; }
    }
}