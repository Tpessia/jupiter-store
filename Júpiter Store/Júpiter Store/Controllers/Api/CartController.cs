using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Xml;
using Júpiter_Store.Dtos;
using Júpiter_Store.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;
using WebGrease.Css.Extensions;
using TransactionStatus = Uol.PagSeguro.Enums.TransactionStatus;

namespace Júpiter_Store.Controllers.Api
{
    public class CartController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId = HttpContext.Current.User.Identity.GetUserId();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountCredentials _credentials = PagSeguroConfiguration.Credentials();
        private Cart ActiveCart
        {
            get
            {
                return _context.Users
                    .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                    .Single(u => u.Id == _userId)
                    .Carts.SingleOrDefault(c => c.IsActive);
            }
        }

        public CartController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
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

            return Ok(new CartAndProductDataDto { CartFinalPrice = ActiveCart.GetFinalPrice(), ProductQuantity = newQuantity });
        }

        // DELETE: Api/Cart/1
        [HttpDelete]
        public IHttpActionResult RemoveItem(int id, int? quantity = 1)
        {
            if (quantity < 1)
                return BadRequest("Invalid quantity. Should be an integer higher than 0.");


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

            return Ok(new CartAndProductDataDto { CartFinalPrice = ActiveCart.GetFinalPrice(), ProductQuantity = newQuantity });
        }

        // POST: Api/Cart/Order
        [HttpPost]
        [Route("Api/Cart/Order")]
        public IHttpActionResult PlaceOrder()
        {
            if (!ActiveCart.Products.Any())
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
                .Include(u => u.Carts)
                .Single(u => u.Id == _userId)
                .Carts.Add(new Cart
                {
                    IsActive = true
                });

            _context.SaveChanges();

            return Ok();
        }

        // POST: Api/Cart/Checkout
        [HttpPost]
        [Route("Api/Cart/Checkout")]
        public IHttpActionResult Checkout()
        {
            // Check for empty cart
            if (!ActiveCart.Products.Any())
                return NotFound();


            // Check for valid stock
            foreach (var cartProduct in ActiveCart.Products)
            {
                if (cartProduct.Quantity > cartProduct.Product.NumberInStock)
                    return BadRequest($"Quantidade no carrinho é maior do que o estoque ({cartProduct.Product.Name})");
            }


            // PagSeguro Logic
            //var address = new Address
            //{
            //    Country = "BRA",
            //    State = "SP",
            //    City = "São Paulo",
            //    District = "Jardim Paulistano",
            //    PostalCode = "01452002",
            //    Street = "Av. Brig. Faria Lima",
            //    Number = "1384",
            //    Complement = "5o. Andar"
            //};

            var user = _userManager.FindById(_userId);
            var paymentRequest = new PaymentRequest
            {
                Currency = Currency.Brl,
                Sender = new Sender(user.UserName, "c41254692078685555836@sandbox.pagseguro.com.br", new Phone(
                    Regex.Match(user.PhoneNumber, @"^\((.*?)\)").Value.Trim('(', ')'),
                    user.PhoneNumber.Substring(user.PhoneNumber.IndexOf(')') + 1)
                    )),
                //Shipping = new Shipping
                //{
                //    Address = address,
                //    Cost = 10.00m,
                //    ShippingType = ShippingType.Pac
                //},
                //ExtraAmount = 10.00m,
                Reference = ActiveCart.Id.ToString(),
                RedirectUri = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/PurchaseHistory/Details/" + ActiveCart.Id),
                MaxAge = 172800, // 2 dias
                NotificationURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Api/Cart/ReceiveNotification"
            };

            foreach (var cartProduct in ActiveCart.Products)
            {
                paymentRequest.Items.Add(
                    new Item(cartProduct.ProductId.ToString(), cartProduct.Product.Name, cartProduct.Quantity, (decimal)cartProduct.Product.Price)
                );
            }

            var paymentRedirectUri = paymentRequest.Register(_credentials);


            // Update Cart/Products & Generate New Cart
            foreach (var cartProduct in ActiveCart.Products)
            {
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


            return Ok(paymentRedirectUri);
        }

        // POST: Api/Cart/Notification
        [HttpPost]
        [Route("Api/Cart/ReceiveNotification")]
        public IHttpActionResult ReceiveNotification(string notificationType, string notificationCode)
        {
            if (notificationType == "transaction")
            {
                // obtendo o objeto transaction a partir do código de notificação  
                var transaction = NotificationService.CheckTransaction(
                    _credentials,
                    notificationCode
                );

                var cart = _context.Users
                    .Include(u => u.Carts.Select(c => c.Products.Select(p => p.Product)))
                    .Single(u => u.Id == _userId)
                    .Carts.Single(c => c.Id == Int32.Parse(transaction.Reference));

                cart.TransactionCode = transaction.Code;

                if (transaction.TransactionStatus == TransactionStatus.Refunded || transaction.TransactionStatus == TransactionStatus.Cancelled)
                {
                    // Return products to stock
                    foreach (var cartProduct in cart.Products)
                    {
                        cartProduct.Product.NumberInStock += cartProduct.Quantity;
                    }
                }
            }

            return Ok();
        }

        // https://ws.sandbox.pagseguro.uol.com.br/v2/transactions?email=thiago.pessia@gmail.com&token=B247180904E540AFA225FCE4D559AFE1&reference=REF1234
        // https://ws.sandbox.pagseguro.uol.com.br/v3/transactions/8CAE1EFC-CA4D-419C-9EA2-27ECEA584D65?email=thiago.pessia@gmail.com&token=B247180904E540AFA225FCE4D559AFE1


        //[HttpPost]
        //[Route("Api/Cart/Checkout")]
        //public IHttpActionResult Checkout()
        //{
        //    //string paymentUri = @"https://pagseguro.uol.com.br/v2/checkout/payment.html"; // Live
        //    //string checkoutUri = @"https://ws.pagseguro.uol.com.br/v2/checkout"; // Live

        //    string paymentUri = @"https://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html"; // Sandbox
        //    string checkoutUri = @"https://ws.sandbox.pagseguro.uol.com.br/v2/checkout"; // Sandbox

        //    var postData = new System.Collections.Specialized.NameValueCollection
        //    {
        //        {"email", "thiago.pessia@gmail.com"},
        //        {"token", "B247180904E540AFA225FCE4D559AFE1"}, // Sandbox
        //        //{"token", "E14085135E404685AAD1CA67B57E474F"}, // Live
        //        {"currency", "BRL"},
        //        {"itemId1", "0001"},
        //        {"itemDescription1", "ProdutoPagSeguroI"},
        //        {"itemAmount1", "3.00"},
        //        {"itemQuantity1", "1"},
        //        {"itemWeight1", "200"},
        //        {"reference", "REF1234"},
        //        {"senderName", "Jose Comprador"},
        //        {"senderAreaCode", "44"},
        //        {"senderPhone", "999999999"},
        //        {"senderEmail", "c41254692078685555836@sandbox.pagseguro.com.br"},
        //        {"shippingAddressRequired", "false"}
        //    };


        //    string xmlString;

        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

        //        var result = wc.UploadValues(checkoutUri, postData);

        //        xmlString = Encoding.ASCII.GetString(result);
        //    }


        //    var xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xmlString);

        //    var code = xmlDoc.GetElementsByTagName("code")[0];
        //    var date = xmlDoc.GetElementsByTagName("date")[0];

        //    var codeText = code.InnerText;
        //    //ActiveCart.TransactionCode = $"{codeText.Substring(0, 8)}-{codeText.Substring(8, 4)}-{codeText.Substring(12, 4)}-{codeText.Substring(16, 4)}-{codeText.Substring(20)}";
        //    ActiveCart.TransactionCode = codeText;
        //    _context.SaveChanges();

        //    var paymentUrl = $"{paymentUri}?code={code.InnerText}";

        //    return Ok(paymentUrl);
        //}
    }
}
