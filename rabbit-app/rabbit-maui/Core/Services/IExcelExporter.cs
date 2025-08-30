using rabbit_maui.Core.Models;

namespace rabbit_maui.Core.Services;

/// <summary>
/// Contract to build the Rabbit-compatible XLSX in memory.
/// Implementation will return a byte[] with a workbook that has a single
/// sheet named "Sections" and exactly these columns:
///   ZR NAME | FROM | TO | SPEED 1
/// </summary>
public interface IExcelExporter
{
    /// <summary>
    /// Builds the XLSX workbook for the given rally.
    /// - ZR NAME: E{n}-T{m}
    /// - FROM: always 0.000
    /// - TO: segment distance in km, rounded to 3 decimals
    /// - SPEED 1: computed speed in km/h, rounded to 1 decimal
    /// </summary>
    Task<byte[]> BuildSectionsWorkbookAsync(Rally rally, CancellationToken ct = default);
}
