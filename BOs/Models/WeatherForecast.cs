using System;
using System.Collections.Generic;

namespace BOs.Models;

public partial class WeatherForecast
{
    public int ForecastId { get; set; }

    public string? Location { get; set; }

    public DateOnly? ForecastDate { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Humidity { get; set; }

    public string? WeatherCondition { get; set; }

    public DateTime? CreatedAt { get; set; }
}
