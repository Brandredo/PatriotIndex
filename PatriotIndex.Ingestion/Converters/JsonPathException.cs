namespace PatriotIndex.Ingestion.Converters;

public sealed class JsonPathException : Exception
{
    public string JsonPath { get; }
    public string PropertyName { get; }

    public JsonPathException(string jsonPath, string propertyName, string parentSnippet)
        : base($"JSON property '{propertyName}' not found at path '{jsonPath}'. Parent: {Truncate(parentSnippet, 200)}")
    {
        JsonPath = jsonPath;
        PropertyName = propertyName;
    }

    public JsonPathException(string jsonPath, string message, Exception inner)
        : base($"JSON parse error at path '{jsonPath}': {message}", inner)
    {
        JsonPath = jsonPath;
        PropertyName = string.Empty;
    }

    private static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..max] + "...";
}
