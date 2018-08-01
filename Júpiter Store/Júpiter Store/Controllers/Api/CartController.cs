using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Júpiter_Store.Dtos;
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
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == userId)
                ?.Carts.SingleOrDefault(c => c.IsActive);

            if (cart == null)
                return NotFound();

            return Ok(new CartDto(cart));
        }

        // POST: Api/Cart
        [HttpPost]
        public IHttpActionResult AddProduct(int id, int? quantity = 1)
        {
            if (quantity < 1)
                return BadRequest("Invalid quantity. Should be an integer higher than 0.");


            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == userId)
                ?.Carts.SingleOrDefault(c => c.IsActive);

            if (cart == null)
                return NotFound();


            int newQuantity;

            if (cart.Products.Any(p => p.ProductId == id)) // Increment Product
            {
                newQuantity = cart.Products.Single(p => p.ProductId == id).Quantity += quantity.GetValueOrDefault();
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

                newQuantity = 1;
            }

            
            _context.SaveChanges();

            return Ok(new CartAndProductDataDto { CartFinalPrice = cart.FinalPrice, ProductQuantity = newQuantity });
        }

        // DELETE: Api/Cart/1
        [HttpDelete]
        public IHttpActionResult RemoveItem(int id, int? quantity = 1)
        {
            if (quantity < 1)
                return BadRequest("Invalid quantity. Should be an integer higher than 0.");


            var userId = User.Identity.GetUserId();

            if (userId == null)
                return NotFound();


            var cart = _context.Users
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == userId)
                ?.Carts.SingleOrDefault(c => c.IsActive);

            if (cart == null)
                return NotFound();


            int newQuantity;

            var productCart = cart.Products.SingleOrDefault(p => p.ProductId == id);

            if (productCart == null)
                return NotFound();

            if (productCart.Quantity > quantity) // Has More Items
            {
                newQuantity = productCart.Quantity -= quantity.GetValueOrDefault();
            }
            else if (productCart.Quantity == quantity) // Last Item
            {
                cart.Products.Remove(productCart);

                newQuantity = 0;
            }
            else
            {
                return BadRequest("Invalid quantity. Subtraction would result in a negative number.");
            }


            _context.SaveChanges();

            return Ok(new CartAndProductDataDto { CartFinalPrice = cart.FinalPrice, ProductQuantity = newQuantity });
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
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == userId)
                ?.Carts.SingleOrDefault(c => c.IsActive);

            if (cart == null || !cart.Products.Any())
                return NotFound();


            foreach (var cartProduct in cart.Products)
            {
                if (cartProduct.Quantity > cartProduct.Product.NumberInStock)
                    return BadRequest($"Quantidade no carrinho é maior do que o estoque ({cartProduct.Product.Name})");

                cartProduct.Product.NumberInStock -= cartProduct.Quantity;
            }


            cart.IsActive = false;
            _context.Users
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == userId)
                .Carts.Add(new Cart
                {
                    IsActive = true
                });

            _context.SaveChanges();

            return Ok();
        }
    }
}
