using System.Text.Json;
using rabbit_maui.Core.Models;

namespace rabbit_maui.Core.Services;

/// <summary>
/// JSON-based persistence for Rally objects.
/// Writes/reads a single file on local storage.
/// </summary>
public class JsonRallyStore : IRallyStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true // pretty JSON for easier debugging
    };

    /// <summary>
    /// Serialize the rally to JSON and write it to the specified absolute path.
    /// </summary>
    public async Task SaveAsync(Rally rally, string path, CancellationToken ct = default)
    {
        // Ensure directory exists
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(rally, Options);
        await File.WriteAllTextAsync(path, json, ct);
    }

    /// <summary>
    /// Read JSON from the specified absolute path and deserialize it to a Rally.
    /// </summary>
    public async Task<Rally> LoadAsync(string path, CancellationToken ct = default)
    {
        var json = await File.ReadAllTextAsync(path, ct);
        var rally = JsonSerializer.Deserialize<Rally>(json, Options);
        if (rally is null)
            throw new InvalidDataException("Unable to deserialize Rally from JSON.");
        return rally;
    }
}
