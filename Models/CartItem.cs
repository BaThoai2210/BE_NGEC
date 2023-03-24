﻿namespace Ecomm.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; } = 0;
    }
}
