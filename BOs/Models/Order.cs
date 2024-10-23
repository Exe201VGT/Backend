using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? BuyerId { get; set; }

    public int? SellerId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

    public virtual Seller? Seller { get; set; }
}
