using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Dtos
{
    public class AddressDto
    {
        [Required]
        [RegularExpression(@"^\d{5}-?\d{3}$")]
        public string PostalCode { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$")]
        public int Number { get; set; }

        public string Complement { get; set; }
    }
}