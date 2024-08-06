﻿using ValeShop.Models.Entities;

namespace ValeShop.Models
{
    public class CartItemViewModel
    {

        public List<CartViewModel> CartItems { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
