﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Júpiter_Store.Models;

namespace Júpiter_Store.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public List<ProductDto> Products { get; set; }

        public CartDto(Cart cart)
        {
            Id = cart.Id;
            Products = new List<ProductDto>();

            foreach (var productCart in cart.Products)
            {
                Products.Add(new ProductDto(productCart));
            }
        }
    }
}