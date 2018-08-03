using System;
using System.Collections.Generic;
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
        public List<string> Roles { get; set; }
    }
}