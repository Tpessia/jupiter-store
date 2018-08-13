using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;

namespace Júpiter_Store.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public List<ProductViewModel> Products { get; set; }
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
                    finalPrice += product.Price * product.Quantity;
                }

                return finalPrice;
            }
        }
        public string ReferenceCode
        {
            get { return $"{Id}_{CreationDate.ToString("yyyyMMddHHmmss")}"; }
        }

        public CartViewModel(Cart cart)
        {
            Id = cart.Id;
            Products = new List<ProductViewModel>();
            IsActive = cart.IsActive;
            CreationDate = cart.CreationDate;
            PurchaseDate = cart.PurchaseDate;
            CheckoutUrl = cart.CheckoutUrl;
            TransactionCode = cart.TransactionCode;

            foreach (var productCart in cart.Products)
            {
                Products.Add(new ProductViewModel(productCart));
            }
        }

        public string GetFinalPrice()
        {
            return $"R$ {FinalPrice}";
        }
    }
}