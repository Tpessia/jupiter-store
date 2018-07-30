using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Júpiter_Store.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public double Price { get; set; }

        public int NumberInStock { get; set; }

        public DateTime DateAdded { get; set; }

        public ICollection<ProductCart> CartsBelonging { get; set; }



        public string GetPrice()
        {
            return $"R$ {Price}";
        }
    }
}