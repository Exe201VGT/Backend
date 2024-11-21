using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string Code { get; set; } = null!;

    public decimal? DiscountPercentage { get; set; }

    public decimal? DiscountAmount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? VoucherImage { get; set; }

    public int? MaxUsage { get; set; }

    public int? UsedCount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
