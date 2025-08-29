namespace rabbit_maui.Core.Models;

public class Segment
{
    public string Id { get; set; } = "";          // E{n}-T{m}

    // Nullable so Blazor can represent an empty input without throwing/parsing issues
    public double? DistanceKm { get; set; }       // increments by 0.1 (UI)
    public double? TimeMin { get; set; }          // increments by 1 (UI)

    // Safe speed calculation: return 0 when values are missing or non-positive
    public double SpeedKmh
    {
        get
        {
            if (DistanceKm is null || TimeMin is null) return 0.0;
            if (DistanceKm <= 0 || TimeMin <= 0) return 0.0;
            return Math.Round((DistanceKm.Value / TimeMin.Value) * 60.0, 1);
        }
    }
}
