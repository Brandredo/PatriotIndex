using System.Text.Json;

namespace PatriotIndex.Ingestion.Converters;

public readonly struct JsonNavigator
{
    private readonly JsonElement _element;
    public string Path { get; }

    public JsonNavigator(JsonElement element, string path = "$")
    {
        _element = element;
        Path = path;
    }

    public JsonNavigator this[string prop]
    {
        get
        {
            if (!_element.TryGetProperty(prop, out var child))
                throw new JsonPathException(Path, prop, _element.ToString());
            return new JsonNavigator(child, $"{Path}.{prop}");
        }
    }

    public JsonNavigator this[int index]
    {
        get
        {
            try
            {
                var child = _element[index];
                return new JsonNavigator(child, $"{Path}[{index}]");
            }
            catch (Exception ex)
            {
                throw new JsonPathException(Path, $"Index {index} out of range", ex);
            }
        }
    }

    public JsonNavigator? Optional(string prop)
    {
        if (!_element.TryGetProperty(prop, out var child))
            return null;
        return new JsonNavigator(child, $"{Path}.{prop}");
    }

    public IEnumerable<JsonNavigator> EnumerateArray()
    {
        int i = 0;
        foreach (var item in _element.EnumerateArray())
        {
            yield return new JsonNavigator(item, $"{Path}[{i}]");
            i++;
        }
    }

    public string GetString()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.Null) return string.Empty;
            return _element.GetString() ?? string.Empty;
        }
        catch (InvalidOperationException ex)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public string? GetStringOrNull()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.Null) return null;
            return _element.GetString();
        }
        catch (InvalidOperationException ex)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public int GetInt32()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.String)
                return int.Parse(_element.GetString()!);
            return _element.GetInt32();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException or OverflowException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }
    
    public decimal GetDecimal()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.String)
                return decimal.Parse(_element.GetString()!);
            return _element.GetDecimal();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException or OverflowException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }
    
    
    public double GetDouble()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.String)
                return double.Parse(_element.GetString()!);
            return _element.GetDouble();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException or OverflowException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }
    
    public short GetInt16()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.String)
                return short.Parse(_element.GetString()!);
            return _element.GetInt16();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException or OverflowException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public long GetInt64()
    {
        try
        {
            if (_element.ValueKind == JsonValueKind.String)
                return long.Parse(_element.GetString()!);
            return _element.GetInt64();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException or OverflowException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public bool GetBoolean()
    {
        try
        {
            return _element.GetBoolean();
        }
        catch (InvalidOperationException ex)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public Guid GetGuid()
    {
        try
        {
            return _element.GetGuid();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public DateTime GetDateTime()
    {
        try
        {
            return _element.GetDateTime().ToUniversalTime();
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }

    public JsonElement Element => _element;

    public DateOnly? GetDateOnly()
    {
        try
        {
            return DateOnly.FromDateTime(_element.GetDateTime());
        }
        catch (Exception ex) when (ex is FormatException or InvalidOperationException)
        {
            throw new JsonPathException(Path, ex.Message, ex);
        }
    }
}
