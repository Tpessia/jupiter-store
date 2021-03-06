﻿using System;
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
using Microsoft.Owin.Security;

namespace Júpiter_Store.Controllers
{
    //[Authorize(Roles = RoleName.Admin)]
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly List<RoleViewModel> _roles;

        public UserManagerController()
        {
            _context = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            _roles = _roleManager.Roles
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToList();
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
                    UserRoles = GetRolesByUserId(u.Id)

                });

            return View(users);
        }

        // GET: UserManager/Edit
        public ActionResult Edit(string userId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.Id == userId);

            if (user == null)
                return HttpNotFound();

            
            var userManagerViewModel = new UserManagerViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserRoleIds = GetRolesByUserId(user.Id).Select(r => r.Id).ToList(),
                Roles = _roles
            };

            return View("UserManagerForm", userManagerViewModel);
        }

        // POST: UserManager/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserManagerViewModel newUserData)
        {
            if (!ModelState.IsValid)
                return View("UserManagerForm", newUserData);


            var applicationUser = _userManager.FindById(newUserData.Id);

            if (applicationUser == null)
                return View("UserManagerForm", newUserData);


            _userManager.RemoveFromRoles(applicationUser.Id, _userManager.GetRoles(applicationUser.Id).ToArray());

            if (newUserData.UserRoleIds != null)
            {
                var userRoleNames = newUserData.UserRoleIds
                    .Select(r => _roleManager.FindById(r).Name)
                    .ToArray();

                foreach (var roleName in userRoleNames)
                {
                    if (!_roleManager.RoleExists(roleName))
                        return View("UserManagerForm", newUserData);
                }

                _userManager.AddToRoles(applicationUser.Id, userRoleNames);
            }

            _userManager.Update(applicationUser);


            var currentApplicationUser = _userManager.FindById(User.Identity.GetUserId());
            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = _userManager.CreateIdentity(currentApplicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("Index");
        }



        // Helpers

        private List<RoleViewModel> GetRolesByUserId(string userId)
        {
            return _userManager.GetRoles(userId)
                .Select(r => new RoleViewModel
                {
                    Id = _roleManager.FindByName(r).Id,
                    Name = r
                })
                .ToList();
        }
    }
}