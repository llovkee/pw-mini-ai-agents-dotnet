using var http = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(5)
};

var client = new PCSSClient(http);

var experiment = args.FirstOrDefault();

switch (experiment)
{
    case "EXP-01":
        await ModelComparison.RunAsync(client);
        break;

    case "EXP-02":
        await Prompting.RunAsync(client);
        break;

    case "EXP-03":
        await Temperature.RunAsync(client);
        break;

    case "EXP-04":
        await StructuredOutput.RunAsync(client);
        break;
    case "FINAL":
        await ResultAfterTest.RunAsync(client);
        break;
    default:
        Console.WriteLine("Wybierz EXP-01 ... EXP-04 or FINAL");
        return;
}

Console.WriteLine($"\nWyniki zapisano w {ResultsCsv.FilePath}");
