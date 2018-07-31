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