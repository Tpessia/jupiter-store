using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Models
{
    public class Cart
    {
        public int Id { get; set; }
        
        public ICollection<ProductCart> Products { get; set; }

        public bool IsActive { get; set; }

        public double FinalPrice
        {
            get
            {
                double finalPrice = 0;

                foreach (var product in Products)
                {
                    finalPrice += product.Product.Price * product.Quantity;
                }

                return finalPrice;
            }
        }
    }
}