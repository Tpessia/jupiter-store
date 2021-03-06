﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;
using Júpiter_Store.ViewModels;
using Microsoft.AspNet.Identity;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;

namespace Júpiter_Store.Controllers
{
    [Authorize]
    public class PurchaseHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        private ICollection<Cart> Carts
        {
            get
            {
                return _context.Users
                    .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                    .Single(u => u.Id == _userId)
                    ?.Carts;
            }
        }

        public PurchaseHistoryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: PurchaseHistory
        public ActionResult Index()
        {
            var purchaseHistory = Carts.Where(c => !c.IsActive).Select(c => new CartViewModel(c)).ToList();
            purchaseHistory.Reverse();

            return View(purchaseHistory);
        }

        // GET: PurchaseHistory/Details/1
        public ActionResult Details(int id)
        {
            var cart = Carts.SingleOrDefault(c => c.Id == id);

            if (cart == null)
                return HttpNotFound();

            var credentials = PagSeguroConfiguration.Credentials();
            if (cart.TransactionCode == null)
            {
                var transactionResult = TransactionSearchService.SearchByReference(
                    credentials,
                    cart.ReferenceCode
                );

                foreach (var transactionSummary in transactionResult.Transactions)
                {
                    if (transactionResult.Transactions.Count == 1)
                    {
                        cart.TransactionCode = transactionSummary.Code;
                    }
                }

                _context.SaveChanges();
            }

            var transaction = cart.TransactionCode != null ? TransactionSearchService.SearchByCode(credentials, cart.TransactionCode) : null;

            return View(new PurchaseHistoryViewModel(cart, transaction));
        }
    }
}