using Microsoft.Extensions.Configuration;

namespace PatriotIndex.Domain.Services;

public class SportsApiClient
{
    private readonly HttpClient _client;

    public SportsApiClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _client.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? throw new InvalidOperationException());
        _client.DefaultRequestHeaders.Add("x-api-key",
            configuration["ApiKey"] ?? throw new Exception("ApiKey not set"));
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.Timeout = TimeSpan.FromSeconds(30); // this may interfere with rate limiting/Polly
    }

    public async Task<string> GetAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrWhiteSpace(content) ? throw new Exception("Empty response") : content;
    }
}