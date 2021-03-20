using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Models
{
    public class ContactUs
    {
        
        public int ID { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string UserEmail { get; set; }
        public DateTime SendDate { get; set; }
    }
}
