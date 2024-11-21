using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public int OrderId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string? Carrier { get; set; }

    public string? ShippingStatus { get; set; }

    public decimal? ShippingFee { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
