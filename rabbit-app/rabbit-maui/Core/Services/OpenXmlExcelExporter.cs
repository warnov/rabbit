using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using rabbit_maui.Core.Models;
using System.Globalization;

namespace rabbit_maui.Core.Services;

/// <summary>
/// Builds a Rabbit-compatible XLSX in memory:
/// Sheet name: "Sections"
/// Columns (exactly these 4, in this order):
///   ZR NAME | FROM | TO | SPEED 1
/// - ZR NAME: segment identifier (e.g., E1-T1)
/// - FROM: always 0.000
/// - TO: distance in km, rounded to 3 decimals
/// - SPEED 1: speed in km/h, rounded to 1 decimal
/// </summary>
public class OpenXmlExcelExporter : IExcelExporter
{
    public Task<byte[]> BuildSectionsWorkbookAsync(Rally rally, CancellationToken ct = default)
    {
        using var ms = new MemoryStream();

        using (var doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook, true))
        {
            var wbPart = doc.AddWorkbookPart();
            wbPart.Workbook = new Workbook();

            // Create worksheet with SheetData
            var wsPart = wbPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            wsPart.Worksheet = new Worksheet(sheetData);

            // Create the Sheets collection and the single "Sections" sheet
            var sheets = wbPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = wbPart.GetIdOfPart(wsPart),
                SheetId = 1,
                Name = "Sections"
            };
            sheets.Append(sheet);

            // Header row
            var header = new Row();
            header.Append(
                MakeTextCell("ZR NAME"),
                MakeTextCell("FROM"),
                MakeTextCell("TO"),
                MakeTextCell("SPEED 1")
            );
            sheetData.Append(header);

            // Data rows
            // We prefer the stored Segment.Id (E{n}-T{m}). If missing, we compute a fallback.
            for (int sIdx = 0; sIdx < rally.Stages.Count; sIdx++)
            {
                var stage = rally.Stages[sIdx];

                for (int gIdx = 0; gIdx < stage.Segments.Count; gIdx++)
                {
                    if (ct.IsCancellationRequested) break;

                    var seg = stage.Segments[gIdx];

                    // Skip incomplete/invalid rows (null or <= 0)
                    var dist = seg.DistanceKm ?? 0.0;
                    var time = seg.TimeMin ?? 0.0;
                    if (dist <= 0 || time <= 0) continue;

                    // ZR NAME (prefer existing Id; fallback to E{n}-T{m})
                    var zr = string.IsNullOrWhiteSpace(seg.Id)
                        ? $"E{sIdx + 1}-T{gIdx + 1}"
                        : seg.Id;

                    // Format values
                    var toVal = Math.Round(dist, 3, MidpointRounding.AwayFromZero)
                        .ToString("0.000", CultureInfo.InvariantCulture);
                    var speedVal = seg.SpeedKmh
                        .ToString("0.0", CultureInfo.InvariantCulture);

                    var row = new Row();
                    row.Append(
                        MakeTextCell(zr),                          // ZR NAME (string)
                        MakeNumberCell("0.000"),                   // FROM (always 0.000)
                        MakeNumberCell(toVal),                     // TO (km, 3 decimals)
                        MakeNumberCell(speedVal)                   // SPEED 1 (km/h, 1 decimal)
                    );
                    sheetData.Append(row);
                }
            }

            wbPart.Workbook.Save();
        }

        return Task.FromResult(ms.ToArray());
    }

    // Helpers to build cells (use fully-qualified OpenXML types to avoid ambiguity)
    private static DocumentFormat.OpenXml.Spreadsheet.Cell MakeTextCell(string text)
        => new DocumentFormat.OpenXml.Spreadsheet.Cell
        {
            DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
            CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(text)
        };

    private static DocumentFormat.OpenXml.Spreadsheet.Cell MakeNumberCell(string numberAsText)
        => new DocumentFormat.OpenXml.Spreadsheet.Cell
        {
            DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number,
            CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(numberAsText)
        };

}
