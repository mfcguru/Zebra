using Printers;
using System.Text.Json;

namespace PrinterApp;

static class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var (driver, mode, content) = ParseArgs(args);
            PrinterSettings settings = LoadPrinterSettings();
            await ExecutePrintingAsync(driver, mode, content, settings);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
    }

    private static (PrinterDriver driver, PrinterMode mode, string content) ParseArgs(string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException("Invalid arguments. Usage: <driver> <mode> <content>");

        if (!Enum.TryParse(args[0], true, out PrinterDriver driver))
            throw new ArgumentException($"Invalid printer driver: {args[0]}");

        if (!Enum.TryParse(args[1], true, out PrinterMode mode))
            throw new ArgumentException($"Invalid printer mode: {args[1]}");

        string content = args[2];

        if (content.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) && File.Exists(content))
            content = File.ReadAllText(content);

        return (driver, mode, content);
    }

    private static PrinterSettings LoadPrinterSettings()
    {
        const string settingsFile = "appsettings.json";
        if (!File.Exists(settingsFile))
            throw new FileNotFoundException($"Configuration file '{settingsFile}' not found.");

        string json = File.ReadAllText(settingsFile);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<PrinterSettings>(json, options)
               ?? throw new InvalidOperationException("Invalid printer settings.");
    }

    private static async Task ExecutePrintingAsync(PrinterDriver driver, PrinterMode mode, string content, PrinterSettings settings)
    {
        var printerFactory = new PrinterFactory();
        var printer = printerFactory.CreatePrinterInstance(driver, mode, settings);

        await printer.Print(content);
        Console.WriteLine("Print job completed successfully.");
    }
}
