using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IClient
{
    Task<string> GenerateAsync(
        string model,
        string prompt,
        double temperature,
        object? format = null
    );
}

public sealed class PCSSClient(HttpClient http) : IClient
{
    
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    // format: null (zwykły tekst), "json" albo obiekt JSON Schema
    public async Task<string> GenerateAsync(
        string model,
        string prompt,
        double temperature = 0,
        object? format = null)
    {
        var request = new
        {
            model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            stream = false,
            format,
            options = new
            {
                temperature
            }
        };
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "api-key");

        // var tmp =  JsonSerializer.Serialize(request, Options);
        // Console.WriteLine(tmp);
        // throw new NotImplementedException();
        
        
        using var response = await http.PostAsJsonAsync(
            "https://llm.hpc.psnc.pl/v1/chat/completions",
            request,
            Options);

        response.EnsureSuccessStatusCode();

        // var json = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(json);
        var json = await response.Content.ReadFromJsonAsync<JsonDocument>();


        return json!.RootElement
            .GetProperty("choices")[0]
        .GetProperty("message")
        .GetProperty("content")
        .GetString() ?? "";
    }
}


public sealed class OllamaClient(HttpClient http) : IClient
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    // format: null (zwykły tekst), "json" albo obiekt JSON Schema
    public async Task<string> GenerateAsync(
        string model,
        string prompt,
        double temperature = 0,
        object? format = null)
    {
        var request = new
        {
            model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            stream = false,
            format,
            options = new
            {
                temperature
            }
        };

        using var response = await http.PostAsJsonAsync(
            "http://localhost:11434/api/chat",
            request,
            Options);

        response.EnsureSuccessStatusCode();

        using var json = await response.Content
            .ReadFromJsonAsync<JsonDocument>();

        return json!.RootElement
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";
    }
}
