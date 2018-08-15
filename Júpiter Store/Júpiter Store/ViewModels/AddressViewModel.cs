using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Júpiter_Store.ViewModels
{
    public class AddressViewModel
    {        
        [Display(Name = "CEP")]
        public string PostalCode { get; set; }

        [Display(Name = "Número")]
        public int Number { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }
    }
}