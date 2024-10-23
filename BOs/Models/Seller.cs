using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Seller
{
    public int SellerId { get; set; }

    public int? UserId { get; set; }

    public string? ShopName { get; set; }

    public string? ShopAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public decimal? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

    public virtual User? User { get; set; }
}
