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
        private readonly ApplicationDbContext _context;
        private readonly string _userId = HttpContext.Current.User.Identity.GetUserId();
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

        // GET: Api/Cart
        public IHttpActionResult GetCart()
        {
            if (ActiveCart == null)
                return NotFound();

            return Ok(new CartDto(ActiveCart));
        }

        // POST: Api/Cart
        [HttpPost]
        public IHttpActionResult AddProduct(int id, int? quantity = 1)
        {
            if (quantity < 1)
                return BadRequest("Invalid quantity. Should be an integer higher than 0.");

            if (ActiveCart == null)
                return NotFound();


            int newQuantity;

            if (ActiveCart.Products.Any(p => p.ProductId == id)) // Increment Product
            {
                newQuantity = ActiveCart.Products.Single(p => p.ProductId == id).Quantity += quantity.GetValueOrDefault();
            }
            else // New Product
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                ActiveCart.Products.Add(new ProductCart()
                {
                    Product = product,
                    Cart = ActiveCart,
                    Quantity = 1
                });

                newQuantity = 1;
            }

            
            _context.SaveChanges();

            return Ok(new CartAndProductDataDto { CartFinalPrice = ActiveCart.FinalPrice, ProductQuantity = newQuantity });
        }

        // DELETE: Api/Cart/1
        [HttpDelete]
        public IHttpActionResult RemoveItem(int id, int? quantity = 1)
        {
            if (quantity < 1)
                return BadRequest("Invalid quantity. Should be an integer higher than 0.");


            if (ActiveCart == null)
                return NotFound();


            int newQuantity;

            var productCart = ActiveCart.Products.SingleOrDefault(p => p.ProductId == id);

            if (productCart == null)
                return NotFound();

            if (productCart.Quantity > quantity) // Has More Items
            {
                newQuantity = productCart.Quantity -= quantity.GetValueOrDefault();
            }
            else if (productCart.Quantity == quantity) // Last Item
            {
                ActiveCart.Products.Remove(productCart);

                newQuantity = 0;
            }
            else
            {
                return BadRequest("Invalid quantity. Subtraction would result in a negative number.");
            }


            _context.SaveChanges();

            return Ok(new CartAndProductDataDto { CartFinalPrice = ActiveCart.FinalPrice, ProductQuantity = newQuantity });
        }

        // POST: Api/Cart/Order
        [HttpPost]
        [Route("Api/Cart/Order")]
        public IHttpActionResult PlaceOrder()
        {
            if (ActiveCart == null || !ActiveCart.Products.Any())
                return NotFound();


            foreach (var cartProduct in ActiveCart.Products)
            {
                if (cartProduct.Quantity > cartProduct.Product.NumberInStock)
                    return BadRequest($"Quantidade no carrinho é maior do que o estoque ({cartProduct.Product.Name})");

                cartProduct.Product.NumberInStock -= cartProduct.Quantity;
            }


            ActiveCart.PurchaseDate = DateTime.Now;
            ActiveCart.IsActive = false;
            _context.Users
                .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                .Single(u => u.Id == _userId)
                .Carts.Add(new Cart
                {
                    IsActive = true
                });

            _context.SaveChanges();

            return Ok();
        }
    }
}
