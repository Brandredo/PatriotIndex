using System.Text.Json;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Interfaces;
using StackExchange.Redis;

namespace PatriotIndex.Domain.Services;

public class CacheService(IConnectionMultiplexer mux, ILogger<CacheService> logger) : ICacheService
{
    private readonly IDatabase _db = mux.GetDatabase();
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var cached = await _db.StringGetAsync(key);
        if (cached.HasValue)
            return JsonSerializer.Deserialize<T>(cached.ToString(), _jsonOptions);

        var lockKey = $"lock:{key}";
        var lockToken = Guid.NewGuid().ToString();
        var acquired = await _db.StringSetAsync(lockKey, lockToken, TimeSpan.FromSeconds(10), When.NotExists);

        if (!acquired)
        {
            await Task.Delay(100);
            return await GetOrSetAsync<T>(key, factory, expiry); // Retry after waiting
        }

        try
        {
            // Double-check after acquiring lock
            cached = await _db.StringGetAsync(key);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<T>(cached.ToString(), _jsonOptions);

            var value = await factory();
            if (value is not null)
                await _db.StringSetAsync(key, JsonSerializer.Serialize(value, _jsonOptions), expiry ?? TimeSpan.FromMinutes(10));

            return value;
        }
        finally
        {
            // Only release the lock if we still own it (Lua script ensures atomicity)
            const string releaseLua = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                return redis.call('DEL', KEYS[1])
            else
                return 0
            end";
            await _db.ScriptEvaluateAsync(releaseLua, [(RedisKey)lockKey], [(RedisValue)lockToken]);
        }
    }

    public async Task InvalidateAsync(string key, CancellationToken cancellationToken = default)
    {
        try { await _db.KeyDeleteAsync(key); }
        catch (RedisException ex) { logger.LogWarning(ex, "Cache invalidation failed for {Key}", key); }
    }
}
