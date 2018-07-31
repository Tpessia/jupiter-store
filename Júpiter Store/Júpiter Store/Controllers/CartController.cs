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
    public class CartController : Controller
    {
        private ApplicationDbContext _context;

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
            var userId = User.Identity.GetUserId();

            var cart = _context.Users
                .Include(u => u.Cart.Products.Select(p => p.Product))
                .SingleOrDefault(u => u.Id == userId)
                ?.Cart;

            return View(new CartViewModel(cart));
        }
    }
}