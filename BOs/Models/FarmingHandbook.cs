using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class FarmingHandbook
{
    public int HandbookId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Category { get; set; }

    public DateTime? CreatedAt { get; set; }
}
