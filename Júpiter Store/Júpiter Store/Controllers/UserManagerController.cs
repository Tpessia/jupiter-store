using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Júpiter_Store.Models;
using Júpiter_Store.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Júpiter_Store.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerController()
        {
            _context = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

         



        // GET: UserManager
        public ActionResult Index()
        {
            var users = _context.Users
                .ToList()
                .Select(u => new UserManagerViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = _userManager.GetRoles(u.Id).ToList()
                });
            
            return View(users);
        }

        // GET: UserManager/Edit
        public ActionResult Edit(string userId)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .Select(u => new UserManagerViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = _userManager.GetRoles(u.Id).ToList()
                })
                .SingleOrDefault(u => u.Id == userId);

            if (user == null)
                return HttpNotFound();

            return View("UserManagerForm", user);
        }

        // GET: UserManager/Save
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Save(string userId, string newRole)
        {
            var user = _userManager.FindById(userId);

            if (user == null || !_roleManager.RoleExists(newRole))
                return HttpNotFound();


            _userManager.RemoveFromRoles(user.Id, _userManager.GetRoles(user.Id).ToArray());
            _userManager.AddToRoles(user.Id, newRole);
            _userManager.Update(user);

            return RedirectToAction("Index");
        }
    }
}