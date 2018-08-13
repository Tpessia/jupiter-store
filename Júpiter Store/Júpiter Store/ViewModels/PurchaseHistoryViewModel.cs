using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;
using Uol.PagSeguro.Domain;

namespace Júpiter_Store.ViewModels
{
    public class PurchaseHistoryViewModel
    {
        public CartViewModel Cart { get; set; }
        public Transaction Transaction { get; set; }

        public PurchaseHistoryViewModel()
        {

        }

        public PurchaseHistoryViewModel(Cart cart, Transaction transaction)
        {
            Cart = new CartViewModel(cart);
            Transaction = transaction;
        }
    }
}