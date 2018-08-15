using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;

namespace Júpiter_Store.ViewModels
{
    public class CheckoutViewModel
    {
        public CartViewModel Cart { get; set; }
        public AddressViewModel Address { get; set; }

        public CheckoutViewModel()
        {
            
        }

        public CheckoutViewModel(Cart cart)
        {
            Cart = new CartViewModel(cart);
        }
    }
}