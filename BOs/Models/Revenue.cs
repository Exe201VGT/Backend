using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Revenue
{
    public int RevenueId { get; set; }

    public int? SellerId { get; set; }

    public int? OrderId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? RevenueDate { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Seller? Seller { get; set; }
}
