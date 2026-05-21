using System.Net.Http.Json;

namespace MauiApp1.Services;

/// <summary>
/// Typed HTTP client for the CRMDemo API — mirrors the same pattern used in CRMDemo/Web/Helpers/ApiClient.cs.
/// Registered via AddHttpClient&lt;ApiClient&gt; so the HttpClient is managed by IHttpClientFactory.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<T>> GetListAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<T>>(endpoint) ?? [];
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, body);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task PostAsync<TRequest>(string endpoint, TRequest body)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, body);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync<TRequest>(string endpoint, TRequest body)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, body);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest body)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, body);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}
