using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;

namespace Júpiter_Store.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;

        public ProductsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }



        // GET: Products
        [AllowAnonymous]
        public ActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        // GET: Products/New
        public ActionResult New()
        {
            var product = new Product();

            return View("ProcuctForm", product);
        }

        // GET: Products/Edit
        public ActionResult Edit(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
                return HttpNotFound();

            return View("ProcuctForm", product);
        }

        // POST: Products/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Product product, HttpPostedFileBase imageFile)
        {
            if (!ModelState.IsValid)
                return View("ProcuctForm", product);

            // New Product
            if (product.Id == 0)
            {
                product.DateAdded = DateTime.Now;

                var newProduct = _context.Products.Add(product);

                if (imageFile != null)
                {
                    newProduct.ImagePath = SaveProductImage(newProduct, imageFile);
                }

                _context.SaveChanges();

                return Redirect("Index");
            }

            // Edit Product
            if (imageFile != null)
            {
                product.ImagePath = SaveProductImage(product, imageFile);
            }

            var productInDb = _context.Products.Single(p => p.Id == product.Id);

            productInDb.Name = product.Name;
            productInDb.Description = product.Description;
            productInDb.ImagePath = product.ImagePath ?? productInDb.ImagePath;
            productInDb.NumberInStock = product.NumberInStock;
            productInDb.Price = product.Price;

            _context.SaveChanges();

            return Redirect("Index");
        }

        public string SaveProductImage(Product product, HttpPostedFileBase imageFile)
        {
            var path = Path.Combine(@"~\Images", product.Name.Replace(" ", "_") + "_" + product.Id + Path.GetExtension(imageFile.FileName));

            imageFile.SaveAs(Server.MapPath(path));

            return path;
        }

        // GET: Products/CheckOut

    }
}