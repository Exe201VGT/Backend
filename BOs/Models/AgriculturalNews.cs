using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class AgriculturalNews
{
    public int NewsId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Source { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public DateTime? CreatedAt { get; set; }
}
