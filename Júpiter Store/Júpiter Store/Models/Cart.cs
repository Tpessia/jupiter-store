using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Uol.PagSeguro.Domain;

namespace Júpiter_Store.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<ProductCart> Products { get; set; }
        public CartShipping Shipping { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string CheckoutUrl { get; set; }
        public string TransactionCode { get; set; }

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
        public string ReferenceCode
        {
            get { return $"{Id}_{CreationDate.ToString("yyyyMMddHHmmss")}"; }
        }

        public string GetFinalPrice()
        {
            return $"R$ {FinalPrice}";
        }
    }
}