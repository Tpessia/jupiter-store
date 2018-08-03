using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Júpiter_Store.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Júpiter_Store.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagerController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: UserManager
        public ActionResult Index()
        {
            var users = _context.Users.Include(u => u.Roles);

            return View(users);
        }

        // GET: UserManager/Edit
        public ActionResult Edit(string userId)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Id == userId);

            if (user == null)
                return HttpNotFound();

            return View("UserManagerForm", user);
        }

        // GET: UserManager/Save
        public ActionResult Save(string userId, string newRole)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Id == userId);

            if (user == null)
                return HttpNotFound();


            //var propertyInfo = typeof(RoleName).GetProperty(newRole);

            //if (propertyInfo == null)
            //    return HttpNotFound();


            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            userManager.RemoveFromRoles(user.Id, user.Roles.Select(r => r.ToString()).ToArray());
            userManager.AddToRole(user.Id, newRole);

            return RedirectToAction("Index");
        }
    }
}