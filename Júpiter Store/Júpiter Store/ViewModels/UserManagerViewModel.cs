using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Júpiter_Store.ViewModels
{
    public class UserManagerViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "User Roles")]
        public List<string> UserRoleIds { get; set; }

        public List<RoleViewModel> UserRoles { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
}