using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;

namespace Júpiter_Store.Controllers
{
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
        public ActionResult Save(Product product, HttpPostedFileBase imageFile)
        {
            if (!ModelState.IsValid)
                return View("ProcuctForm", product);

            if (imageFile != null)
            {
                var path = Path.Combine(@"~\Images", product.Name + "_" + product.Id + Path.GetExtension(imageFile.FileName));

                imageFile.SaveAs(Server.MapPath(path));

                product.ImagePath = path;
            }

            // New Product
            if (product.Id == 0)
            {
                product.DateAdded = DateTime.Now;

                _context.Products.Add(product);
                _context.SaveChanges();

                return Redirect("Index");
            }
            
            // Edit Product
            var productInDb = _context.Products.Single(p => p.Id == product.Id);

            productInDb.Name = product.Name;
            productInDb.Description = product.Description;
            productInDb.ImagePath = product.ImagePath;
            productInDb.NumberInStock = productInDb.NumberInStock;
            productInDb.Price = product.Price;

            _context.SaveChanges();

            return Redirect("Index");
        }

        // GET: Products/CheckOut

    }
}