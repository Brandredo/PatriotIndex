using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PatriotIndex.Domain.Services;

public class SportsApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<SportsApiClient> _logger;

    public SportsApiClient(HttpClient client, IConfiguration configuration, ILogger<SportsApiClient> logger)
    {
        _client = client;
        _logger = logger;
        _client.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? throw new InvalidOperationException());
        _client.DefaultRequestHeaders.Add("x-api-key",
            configuration["ApiKey"] ?? throw new Exception("ApiKey not set"));
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.Timeout = TimeSpan.FromSeconds(30); // this may interfere with rate limiting/Polly
    }

    public async Task<string> GetAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync(endpoint, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _logger.LogError("SportRadar API returned {StatusCode} for {Endpoint}", response.StatusCode, endpoint);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrWhiteSpace(content) ? throw new Exception("Empty response") : content;
    }
}
