using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public int? VoucherId { get; set; }

    public decimal TotalAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? ShippingStatus { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

    public virtual Seller Seller { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual Voucher? Voucher { get; set; }
}
