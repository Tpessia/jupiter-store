using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using Júpiter_Store.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Júpiter_Store.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindByName(HttpContext.Current.User.Identity.Name);

            if (user == null)
                throw new AuthenticationException();

            return user.FirstName;
        }

        public static string GetLastName(this IIdentity identity)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindByName(HttpContext.Current.User.Identity.Name);

            if (user == null)
                throw new AuthenticationException();

            return user.LastName;
        }

        public static string GetFullName(this IIdentity identity)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindByName(HttpContext.Current.User.Identity.Name);

            if (user == null)
                throw new AuthenticationException();

            return $"{user.FirstName} {user.LastName}";
        }
    }
}