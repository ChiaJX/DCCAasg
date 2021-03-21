using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Models
{
    public class Cart
    {
        public int ID { get; set; }

        public string ProdName { get; set; }

        public double ProdPrice { get; set; }

        public string ProdImgUrl { get; set; }

        public string ProdQty { get; set; }

    }
}
