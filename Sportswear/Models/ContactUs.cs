using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Models
{
    public class ContactUs
    {
        
        public int ID { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public DateTime SendDate { get; set; }
    }
}
