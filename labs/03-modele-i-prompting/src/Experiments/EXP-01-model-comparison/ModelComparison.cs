// EXP-01: porównanie dwóch modeli w ekstrakcji piętra i windy
public static class ModelComparison
{
    private static readonly string[] Models =
    [
        // "bielik_11b",
        "DeepSeek-V3.1",
        "DeepSeek-V4-Flash",
        "Qwen3.5-397B-A17B"
        
    ];

    public static async Task RunAsync(IClient client)
    {
        foreach (var model in Models)
        {
            Console.WriteLine($"\nModel: {model}");

            var metrics = await Extraction.EvaluateAsync(
                client,
                model,
                TestData.Examples,
                note => Extraction.Prompt(note));

            ResultsCsv.Append("EXP-01", model, "zero-shot", 0, metrics);
        }
    }
}
