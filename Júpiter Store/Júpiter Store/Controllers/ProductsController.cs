using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace Júpiter_Store.Controllers
{
    [System.Web.Mvc.Authorize(Roles = RoleName.Manager)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }



        // GET: Products
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            var products = _context.Products.ToList();
            products.Reverse();

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
        [System.Web.Mvc.HttpPost]
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
                    newProduct.ImagePath = GetProductImagePath(newProduct, imageFile);

                    imageFile.SaveAs(Server.MapPath(newProduct.ImagePath));
                }

                _context.SaveChanges();

                return Redirect("Index");
            }

            // Edit Product
            if (imageFile != null)
            {
                product.ImagePath = GetProductImagePath(product, imageFile);

                imageFile.SaveAs(Server.MapPath(product.ImagePath));
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

        // POST: Products/Delete/1
        public ActionResult Delete(int id)
        {
            var product = _context.Products.Include(p => p.CartsBelonging).SingleOrDefault(p => p.Id == id);

            if (product == null)
                return HttpNotFound();

            if (product.CartsBelonging.Any())
                throw new HttpResponseException(HttpStatusCode.Conflict);

            if (System.IO.File.Exists(Server.MapPath(product.ImagePath)))
            {
                var path = Server.MapPath(product.ImagePath);

                _context.Products.Remove(product);
                _context.SaveChanges();

                System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }


        // Helpers

        public string GetProductImagePath(Product product, HttpPostedFileBase imageFile)
        {
            return Path.Combine(@"~\Public\Images\Products", product.Name.Replace(" ", "_") + "_" + product.DateAdded.ToString("yyyyMMddHHmmss") + Path.GetExtension(imageFile.FileName));
        }

    }
}