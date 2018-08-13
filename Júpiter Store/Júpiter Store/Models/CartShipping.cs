using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Uol.PagSeguro.Domain;

namespace Júpiter_Store.Models
{
    public class CartShipping : Shipping
    {
        [ForeignKey("Cart")]
        public int Id { get; set; }
        public Cart Cart { get; set; }
    }
}