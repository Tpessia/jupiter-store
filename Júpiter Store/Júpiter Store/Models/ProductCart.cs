using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Models
{
    public class ProductCart
    {
        [Key, Column(Order = 0), ForeignKey("Product")]
        public int ProductId { get; set; }
        [Key, Column(Order = 1), ForeignKey("Cart")]
        public int CartId { get; set; }

        public Product Product { get; set; }
        public Cart Cart { get; set; }

        public int Quantity { get; set; }
    }
}