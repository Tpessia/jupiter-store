using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;

namespace Júpiter_Store.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public double TotalPrice
        {
            get { return Price * Quantity; }
        }

        public ProductDto(ProductCart productCart)
        {
            Id = productCart.Product.Id;
            Name = productCart.Product.Name;
            Description = productCart.Product.Description;
            ImagePath = productCart.Product.ImagePath;
            Price = productCart.Product.Price;
            Quantity = productCart.Quantity;
        }
    }
}