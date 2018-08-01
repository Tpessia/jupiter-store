using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Dtos
{
    public class CartAndProductDataDto
    {
        public double CartFinalPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}