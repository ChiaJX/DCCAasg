using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Models
{
    public class Transaction
    {
        public int transactionId { get; set; }

        [Required]
        public string userId { get; set; }
        [Required]
        public string userAddress { get; set; }
        [Required]
        public string userPhone { get; set; }
        [Required]
        public string orderId { get; set; }
        [Required]
        public string product { get; set; }
        [Required]
        public string couponId { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        //[Range(1, 100, ErrorMessage = "Price entered must be within 1 to 100")]
        public decimal price { get; set; }

        [Required]
        public DateTime TransactionDateTime { get; set; }

    }
}
