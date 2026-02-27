namespace PatriotIndex.Domain.Helpers;

using System.Text.Json;

public sealed class JsonTraverser : IDisposable
{
    private readonly JsonDocument _doc;
    private readonly JsonElement  _root;

    public JsonTraverser(string json)
    {
        _doc  = JsonDocument.Parse(json);
        _root = _doc.RootElement;
    }

    public JsonElement?  Navigate  (string path) => JsonTraverserCore.Navigate(_root, path);
    public JsonTraverserItem? Scope(string path)  => JsonTraverserCore.Scope(_root, path);

    // --- Primitives ---
    public string      GetString     (string path, string   fallback = "")    => JsonTraverserCore.GetString     (_root, path, fallback);
    public bool        GetBool       (string path, bool     fallback = false)  => JsonTraverserCore.GetBool       (_root, path, fallback);
    public byte        GetByte       (string path, byte     fallback = 0)      => JsonTraverserCore.GetByte       (_root, path, fallback);
    public short       GetShort      (string path, short    fallback = 0)      => JsonTraverserCore.GetShort      (_root, path, fallback);
    public int         GetInt        (string path, int      fallback = 0)      => JsonTraverserCore.GetInt        (_root, path, fallback);
    public long        GetLong       (string path, long     fallback = 0)      => JsonTraverserCore.GetLong       (_root, path, fallback);
    public float       GetFloat      (string path, float    fallback = 0)      => JsonTraverserCore.GetFloat      (_root, path, fallback);
    public double      GetDouble     (string path, double   fallback = 0)      => JsonTraverserCore.GetDouble     (_root, path, fallback);
    public decimal     GetDecimal    (string path, decimal  fallback = 0)      => JsonTraverserCore.GetDecimal    (_root, path, fallback);
    public uint        GetUInt       (string path, uint     fallback = 0)      => JsonTraverserCore.GetUInt       (_root, path, fallback);
    public ulong       GetULong      (string path, ulong    fallback = 0)      => JsonTraverserCore.GetULong      (_root, path, fallback);
    public char        GetChar       (string path, char     fallback = '\0')   => JsonTraverserCore.GetChar       (_root, path, fallback);

    // --- Dates and times ---
    public DateTime?       GetDateTime      (string path) => JsonTraverserCore.GetDateTime      (_root, path);
    public DateTimeOffset? GetDateTimeOffset(string path) => JsonTraverserCore.GetDateTimeOffset(_root, path);
    public TimeSpan?       GetTimeSpan      (string path) => JsonTraverserCore.GetTimeSpan      (_root, path);
    public DateOnly?       GetDateOnly      (string path) => JsonTraverserCore.GetDateOnly      (_root, path);
    public TimeOnly?       GetTimeOnly      (string path) => JsonTraverserCore.GetTimeOnly      (_root, path);

    // --- Other common types ---
    public Guid?    GetGuid   (string path) => JsonTraverserCore.GetGuid   (_root, path);
    public Uri?     GetUri    (string path) => JsonTraverserCore.GetUri    (_root, path);
    public byte[]?  GetBytes  (string path) => JsonTraverserCore.GetBytes  (_root, path);

    // --- Nullable primitives ---
    public bool?    GetBoolN   (string path) => JsonTraverserCore.GetNullable(_root, path, e => e.ValueKind == JsonValueKind.True ? true : e.ValueKind == JsonValueKind.False ? false : (bool?)null);
    public int?     GetIntN    (string path) => JsonTraverserCore.GetNullable(_root, path, e => e.TryGetInt32(out  int i)     ? i : (int?)null);
    public long?    GetLongN   (string path) => JsonTraverserCore.GetNullable(_root, path, e => e.TryGetInt64(out  long l)    ? l : (long?)null);
    public double?  GetDoubleN (string path) => JsonTraverserCore.GetNullable(_root, path, e => e.TryGetDouble(out double d)  ? d : (double?)null);
    public decimal? GetDecimalN(string path) => JsonTraverserCore.GetNullable(_root, path, e => e.TryGetDecimal(out decimal m) ? m : (decimal?)null);
    public float?   GetFloatN  (string path) => JsonTraverserCore.GetNullable(_root, path, e => e.TryGetSingle(out float f)   ? f : (float?)null);

    // --- Enum ---
    public T GetEnum<T>(string path, T fallback = default) where T : struct, Enum => JsonTraverserCore.GetEnum<T>(_root, path, fallback);

    // --- Collections ---
    public IEnumerable<T>                            GetArray     <T>(string path, Func<JsonTraverserItem, T> map) => JsonTraverserCore.GetArray     (_root, path, map);
    public IEnumerable<(string Key, JsonTraverserItem Value)> GetProperties(string path)                           => JsonTraverserCore.GetProperties(_root, path);
    public List<string>  GetStringList(string path) => JsonTraverserCore.GetPrimitiveList(_root, path, e => e.GetString() ?? "");
    public List<int>     GetIntList   (string path) => JsonTraverserCore.GetPrimitiveList(_root, path, e => e.TryGetInt32(out int i) ? i : 0);
    public List<double>  GetDoubleList(string path) => JsonTraverserCore.GetPrimitiveList(_root, path, e => e.TryGetDouble(out double d) ? d : 0);

    public void Dispose() => _doc.Dispose();
}


// -------------------------------------------------------------------------
// Shared traversal logic — used by both JsonTraverser and JsonTraverserItem
// -------------------------------------------------------------------------

internal static class JsonTraverserCore
{
    internal static JsonElement? Navigate(JsonElement element, string path)
    {
        JsonElement current = element;
        foreach (var key in path.Split('.'))
        {
            if (current.ValueKind == JsonValueKind.Array && int.TryParse(key, out int index))
            {
                if (index < 0 || index >= current.GetArrayLength()) return null;
                current = current[index];
            }
            else if (!current.TryGetProperty(key, out current))
            {
                return null;
            }
        }
        return current;
    }

    internal static JsonTraverserItem? Scope(JsonElement root, string path)
    {
        var e = Navigate(root, path);
        return e.HasValue ? new JsonTraverserItem(e.Value) : null;
    }

    // --- Primitives ---

    internal static string  GetString (JsonElement root, string path, string  fallback) => Navigate(root, path)?.GetString() ?? fallback;
    internal static bool    GetBool   (JsonElement root, string path, bool    fallback)
    {
        var e = Navigate(root, path);
        if (e is null) return fallback;
        return e.Value.ValueKind switch
        {
            JsonValueKind.True  => true,
            JsonValueKind.False => false,
            // Handle "true"/"false" stored as strings
            JsonValueKind.String => bool.TryParse(e.Value.GetString(), out bool b) ? b : fallback,
            _ => fallback
        };
    }

    internal static byte    GetByte   (JsonElement root, string path, byte    fallback) => Navigate(root, path) is { } e && e.TryGetByte   (out byte    v) ? v : fallback;
    internal static short   GetShort  (JsonElement root, string path, short   fallback) => Navigate(root, path) is { } e && e.TryGetInt16  (out short   v) ? v : fallback;
    internal static int     GetInt    (JsonElement root, string path, int     fallback) => Navigate(root, path) is { } e && e.TryGetInt32  (out int     v) ? v : fallback;
    internal static long    GetLong   (JsonElement root, string path, long    fallback) => Navigate(root, path) is { } e && e.TryGetInt64  (out long    v) ? v : fallback;
    internal static float   GetFloat  (JsonElement root, string path, float   fallback) => Navigate(root, path) is { } e && e.TryGetSingle (out float   v) ? v : fallback;
    internal static double  GetDouble (JsonElement root, string path, double  fallback) => Navigate(root, path) is { } e && e.TryGetDouble (out double  v) ? v : fallback;
    internal static decimal GetDecimal(JsonElement root, string path, decimal fallback) => Navigate(root, path) is { } e && e.TryGetDecimal(out decimal v) ? v : fallback;
    internal static uint    GetUInt   (JsonElement root, string path, uint    fallback) => Navigate(root, path) is { } e && e.TryGetUInt32 (out uint    v) ? v : fallback;
    internal static ulong   GetULong  (JsonElement root, string path, ulong   fallback) => Navigate(root, path) is { } e && e.TryGetUInt64 (out ulong   v) ? v : fallback;
    internal static char    GetChar   (JsonElement root, string path, char    fallback)
    {
        var s = Navigate(root, path)?.GetString();
        return s?.Length == 1 ? s[0] : fallback;
    }

    // --- Dates and times ---

    internal static DateTime? GetDateTime(JsonElement root, string path)
    {
        var e = Navigate(root, path);
        if (e is null) return null;
        if (e.Value.TryGetDateTime(out DateTime dt)) return dt;
        // Fall back to parsing a string manually
        if (e.Value.ValueKind == JsonValueKind.String &&
            DateTime.TryParse(e.Value.GetString(), out DateTime parsed)) return parsed;
        return null;
    }

    internal static DateTimeOffset? GetDateTimeOffset(JsonElement root, string path)
    {
        var e = Navigate(root, path);
        if (e is null) return null;
        if (e.Value.TryGetDateTimeOffset(out DateTimeOffset dto)) return dto;
        if (e.Value.ValueKind == JsonValueKind.String &&
            DateTimeOffset.TryParse(e.Value.GetString(), out DateTimeOffset parsed)) return parsed;
        return null;
    }

    internal static TimeSpan? GetTimeSpan(JsonElement root, string path)
    {
        var s = Navigate(root, path)?.GetString();
        return s is not null && TimeSpan.TryParse(s, out TimeSpan ts) ? ts : null;
    }

    internal static DateOnly? GetDateOnly(JsonElement root, string path)
    {
        var s = Navigate(root, path)?.GetString();
        return s is not null && DateOnly.TryParse(s, out DateOnly d) ? d : null;
    }

    internal static TimeOnly? GetTimeOnly(JsonElement root, string path)
    {
        var s = Navigate(root, path)?.GetString();
        return s is not null && TimeOnly.TryParse(s, out TimeOnly t) ? t : null;
    }

    // --- Other common types ---

    internal static Guid? GetGuid(JsonElement root, string path)
    {
        var e = Navigate(root, path);
        if (e is null) return null;
        if (e.Value.TryGetGuid(out Guid g)) return g;
        if (e.Value.ValueKind == JsonValueKind.String &&
            Guid.TryParse(e.Value.GetString(), out Guid parsed)) return parsed;
        return null;
    }

    internal static Uri? GetUri(JsonElement root, string path)
    {
        var s = Navigate(root, path)?.GetString();
        return s is not null && Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out Uri? uri) ? uri : null;
    }

    // Handles base64-encoded byte arrays
    internal static byte[]? GetBytes(JsonElement root, string path)
    {
        var e = Navigate(root, path);
        if (e is null) return null;
        if (e.Value.TryGetBytesFromBase64(out byte[]? bytes)) return bytes;
        return null;
    }

    // --- Nullable helper ---

    internal static T? GetNullable<T>(JsonElement root, string path, Func<JsonElement, T?> extractor) where T : struct
    {
        var e = Navigate(root, path);
        if (e is null || e.Value.ValueKind == JsonValueKind.Null) return null;
        return extractor(e.Value);
    }

    // --- Enum ---

    internal static T GetEnum<T>(JsonElement root, string path, T fallback) where T : struct, Enum
    {
        var e = Navigate(root, path);
        if (e is null) return fallback;

        // Try parsing from string name (case-insensitive)
        if (e.Value.ValueKind == JsonValueKind.String)
            return Enum.TryParse<T>(e.Value.GetString(), ignoreCase: true, out T result) ? result : fallback;

        // Try parsing from integer value
        if (e.Value.TryGetInt32(out int i) && Enum.IsDefined(typeof(T), i))
            return (T)(object)i;

        return fallback;
    }

    // --- Collections ---

    internal static IEnumerable<T> GetArray<T>(JsonElement root, string path, Func<JsonTraverserItem, T> map)
    {
        var element = Navigate(root, path);
        if (element is null || element.Value.ValueKind != JsonValueKind.Array)
            yield break;

        foreach (JsonElement item in element.Value.EnumerateArray())
            yield return map(new JsonTraverserItem(item));
    }

    internal static IEnumerable<(string Key, JsonTraverserItem Value)> GetProperties(JsonElement root, string path)
    {
        var element = Navigate(root, path);
        if (element is null || element.Value.ValueKind != JsonValueKind.Object)
            yield break;

        foreach (JsonProperty prop in element.Value.EnumerateObject())
            yield return (prop.Name, new JsonTraverserItem(prop.Value));
    }

    internal static List<T> GetPrimitiveList<T>(JsonElement root, string path, Func<JsonElement, T> extractor)
    {
        var element = Navigate(root, path);
        if (element is null || element.Value.ValueKind != JsonValueKind.Array)
            return [];

        return element.Value.EnumerateArray().Select(extractor).ToList();
    }
}


// -------------------------------------------------------------------------
// Scoped view — mirrors all methods, rooted at a nested node
// -------------------------------------------------------------------------

public sealed class JsonTraverserItem
{
    private readonly JsonElement _element;
    internal JsonTraverserItem(JsonElement element) => _element = element;

    public JsonElement?       Navigate  (string path) => JsonTraverserCore.Navigate  (_element, path);
    public JsonTraverserItem? Scope     (string path) => JsonTraverserCore.Scope     (_element, path);

    // --- Primitives ---
    public string  GetString (string path, string  fallback = "")    => JsonTraverserCore.GetString (_element, path, fallback);
    public bool    GetBool   (string path, bool    fallback = false)  => JsonTraverserCore.GetBool   (_element, path, fallback);
    public byte    GetByte   (string path, byte    fallback = 0)      => JsonTraverserCore.GetByte   (_element, path, fallback);
    public short   GetShort  (string path, short   fallback = 0)      => JsonTraverserCore.GetShort  (_element, path, fallback);
    public int     GetInt    (string path, int     fallback = 0)      => JsonTraverserCore.GetInt    (_element, path, fallback);
    public long    GetLong   (string path, long    fallback = 0)      => JsonTraverserCore.GetLong   (_element, path, fallback);
    public float   GetFloat  (string path, float   fallback = 0)      => JsonTraverserCore.GetFloat  (_element, path, fallback);
    public double  GetDouble (string path, double  fallback = 0)      => JsonTraverserCore.GetDouble (_element, path, fallback);
    public decimal GetDecimal(string path, decimal fallback = 0)      => JsonTraverserCore.GetDecimal(_element, path, fallback);
    public uint    GetUInt   (string path, uint    fallback = 0)      => JsonTraverserCore.GetUInt   (_element, path, fallback);
    public ulong   GetULong  (string path, ulong   fallback = 0)      => JsonTraverserCore.GetULong  (_element, path, fallback);
    public char    GetChar   (string path, char    fallback = '\0')   => JsonTraverserCore.GetChar   (_element, path, fallback);

    // --- Dates and times ---
    public DateTime?       GetDateTime      (string path) => JsonTraverserCore.GetDateTime      (_element, path);
    public DateTimeOffset? GetDateTimeOffset(string path) => JsonTraverserCore.GetDateTimeOffset(_element, path);
    public TimeSpan?       GetTimeSpan      (string path) => JsonTraverserCore.GetTimeSpan      (_element, path);
    public DateOnly?       GetDateOnly      (string path) => JsonTraverserCore.GetDateOnly      (_element, path);
    public TimeOnly?       GetTimeOnly      (string path) => JsonTraverserCore.GetTimeOnly      (_element, path);

    // --- Other common types ---
    public Guid?   GetGuid (string path) => JsonTraverserCore.GetGuid (_element, path);
    public Uri?    GetUri  (string path) => JsonTraverserCore.GetUri  (_element, path);
    public byte[]? GetBytes(string path) => JsonTraverserCore.GetBytes(_element, path);

    // --- Nullable primitives ---
    public bool?    GetBoolN   (string path) => JsonTraverserCore.GetNullable(_element, path, e => e.ValueKind == JsonValueKind.True ? true : e.ValueKind == JsonValueKind.False ? false : (bool?)null);
    public int?     GetIntN    (string path) => JsonTraverserCore.GetNullable(_element, path, e => e.TryGetInt32  (out int     i) ? i : (int?)null);
    public long?    GetLongN   (string path) => JsonTraverserCore.GetNullable(_element, path, e => e.TryGetInt64  (out long    l) ? l : (long?)null);
    public double?  GetDoubleN (string path) => JsonTraverserCore.GetNullable(_element, path, e => e.TryGetDouble (out double  d) ? d : (double?)null);
    public decimal? GetDecimalN(string path) => JsonTraverserCore.GetNullable(_element, path, e => e.TryGetDecimal(out decimal m) ? m : (decimal?)null);
    public float?   GetFloatN  (string path) => JsonTraverserCore.GetNullable(_element, path, e => e.TryGetSingle (out float   f) ? f : (float?)null);

    // --- Enum ---
    public T GetEnum<T>(string path, T fallback = default) where T : struct, Enum => JsonTraverserCore.GetEnum<T>(_element, path, fallback);

    // --- Collections ---
    public IEnumerable<T>                                     GetArray     <T>(string path, Func<JsonTraverserItem, T> map) => JsonTraverserCore.GetArray     (_element, path, map);
    public IEnumerable<(string Key, JsonTraverserItem Value)> GetProperties   (string path)                                => JsonTraverserCore.GetProperties(_element, path);
    public List<string> GetStringList(string path) => JsonTraverserCore.GetPrimitiveList(_element, path, e => e.GetString() ?? "");
    public List<int>    GetIntList   (string path) => JsonTraverserCore.GetPrimitiveList(_element, path, e => e.TryGetInt32(out int i) ? i : 0);
    public List<double> GetDoubleList(string path) => JsonTraverserCore.GetPrimitiveList(_element, path, e => e.TryGetDouble(out double d) ? d : 0);
}