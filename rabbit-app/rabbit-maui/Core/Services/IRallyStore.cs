using rabbit_maui.Core.Models;

namespace rabbit_maui.Core.Services;

/// <summary>
/// Contract for saving/loading a Rally to/from a local file.
/// Implementations can use JSON, XML, etc.
/// </summary>
public interface IRallyStore
{
    /// <summary>
    /// Save the given rally to the provided absolute file path.
    /// </summary>
    Task SaveAsync(Rally rally, string path, CancellationToken ct = default);

    /// <summary>
    /// Load a rally from the provided absolute file path.
    /// </summary>
    Task<Rally> LoadAsync(string path, CancellationToken ct = default);
}
