﻿using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? SellerId { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public decimal? Weight { get; set; }

    public string? Description { get; set; }

    public int? StockQuantity { get; set; }

    public string? ProductImage { get; set; }

    public decimal? AverageRating { get; set; }

    public int? ReviewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Seller? Seller { get; set; }
}
