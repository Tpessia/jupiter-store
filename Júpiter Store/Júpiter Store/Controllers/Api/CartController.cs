using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Júpiter_Store.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace Júpiter_Store.Controllers.Api
{
    public class CartController : ApiController
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

        // GET: Api/Cart
        public IHttpActionResult GetCart()
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Cart.Products.Select(p => p.Product))
                .SingleOrDefault(u => u.Id == userId)
                ?.Cart;

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        // POST: Api/Cart
        [HttpPost]
        public IHttpActionResult AddProduct(int id)
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Cart.Products)
                .SingleOrDefault(u => u.Id == userId)
                ?.Cart;

            if (cart == null)
                return NotFound();


            if (cart.Products.Any(p => p.ProductId == id)) // Increment Product
            {
                cart.Products.Single(p => p.ProductId == id).Quantity++;
            }
            else // New Product
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                cart.Products.Add(new ProductCart()
                {
                    Product = product,
                    Cart = cart,
                    Quantity = 1
                });
            }

            
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: Api/Cart/1
        [HttpDelete]
        public IHttpActionResult RemoveItem(int id)
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Cart.Products)
                .SingleOrDefault(u => u.Id == userId)
                ?.Cart;

            if (cart == null)
                return NotFound();


            var productCart = cart.Products.SingleOrDefault(p => p.ProductId == id);

            if (productCart == null)
                return NotFound();

            if (productCart.Quantity > 1) // Has More Items
            {
                productCart.Quantity--;
            }
            else // Last Item
            {
                cart.Products.Remove(productCart);
            }


            _context.SaveChanges();

            return Ok();
        }

        // POST: Api/Cart/Order
        [HttpPost]
        [Route("Api/Cart/Order")]
        public IHttpActionResult PlaceOrder()
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Cart.Products)
                .SingleOrDefault(u => u.Id == userId)
                ?.Cart;

            if (cart == null)
                return NotFound();


            cart.Products.Clear();
            _context.SaveChanges();

            return Ok();
        }
    }
}
