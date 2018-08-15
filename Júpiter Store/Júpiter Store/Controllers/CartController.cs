using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;
using Júpiter_Store.ViewModels;
using Microsoft.AspNet.Identity;

namespace Júpiter_Store.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        private Cart ActiveCart
        {
            get
            {
                return _context.Users
                    .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                    .Single(u => u.Id == _userId)
                    ?.Carts.SingleOrDefault(c => c.IsActive);
            }
        }

        public CartController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        


        // GET: Cart
        public ActionResult Index()
        {
            return View(new CartViewModel(ActiveCart));
        }

        // GET: Cart/CheckOut
        public ActionResult Checkout()
        {
            if (!ActiveCart.Products.Any())
            {
                return HttpNotFound();
            }

            return View(new CheckoutViewModel(ActiveCart));
        }
    }
}