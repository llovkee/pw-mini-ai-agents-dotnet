public static class ResultAfterTest
{
    private const string Model = "DeepSeek-V4-Flash";

    public static async Task RunAsync(IClient client)
    {
        var format = Extraction.Schema;
        var temp = 0.5;

        var metrics = await Extraction.EvaluateAsync(
            client,
            Model,
            TestData.Notes,
            note => Extraction.Prompt(note, TestData.PromptExamples),
            format: format);

        ResultsCsv.Append("FINAL", Model, "few-shot" , temp, metrics);
    }
}