using System.Text.Json;

namespace PatriotIndex.Domain.Extensions;

// Must be its own top-level static class, not nested inside Worker or any other class
public static class JsonElementExtensions
{
    public static JsonElement SafeGetProperty(this JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value))
            throw new KeyNotFoundException($"Property '{propertyName}' was not found in JSON object: {element}");
        return value;
    }
}