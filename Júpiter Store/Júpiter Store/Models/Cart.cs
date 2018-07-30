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
    }
}