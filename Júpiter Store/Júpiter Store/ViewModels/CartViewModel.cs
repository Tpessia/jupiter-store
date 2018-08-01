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


        public CartViewModel(Cart cart)
        {
            Id = cart.Id;
            Products = new List<ProductViewModel>();

            foreach (var productCart in cart.Products)
            {
                Products.Add(new ProductViewModel(productCart));
            }
        }
    }
}