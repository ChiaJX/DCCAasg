using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Sportswear.Models
{
    public class ProductEntity : TableEntity
    {
        public List<productDetails> Value { get; set; }
    }

    public class productDetails
    {
        public string productName { get; set; }
        public string productPrice { get; set; }
        public string productImgUrl { get; set; }
    }
}
