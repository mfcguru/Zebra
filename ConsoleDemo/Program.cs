using Microsoft.Extensions.Configuration;
using Zebra.Printers;

namespace ConsoleDemo;

static class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var (mode, content) = ParseArgs(args);
            if (string.IsNullOrEmpty(mode) || string.IsNullOrEmpty(content)) return;

            var configuration = LoadConfiguration();
            var printerFactory = new PrinterFactory();
            var printer = CreatePrinterInstance(mode, configuration, printerFactory);
            if (printer == null) return;

            await PrintContent(printer, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while printing: {ex.Message}");
        }
    }

    private static (string? mode, string? content) ParseArgs(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <mode> <content>");
            Console.WriteLine("Modes: 'tcp', 'ws', or 'windows'");
            Console.WriteLine("Content: ZPL string for the printer");
            return (null, null);
        }

        string mode = args[0].ToLower();
        string content = args[1];
        return (mode, content);
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static IPrinter? CreatePrinterInstance(string mode, IConfiguration configuration, PrinterFactory printerFactory)
    {
        IPrinter? printer = null;

        switch (mode)
        {
            case "tcp":
                var tcpConfig = configuration.GetSection("PrinterConfigurations:Tcp");
                printer = printerFactory.CreatePrinterInstance(Mode.Tcp, tcpConfig["IpAddress"]!, int.Parse(tcpConfig["Port"]!));
                break;
            case "ws":
                var wsConfig = configuration["PrinterConfigurations:WebSocket:Url"];
                printer = printerFactory.CreatePrinterInstance(Mode.WebSocket, wsConfig!);
                break;
            case "windows":
                var windowsConfig = configuration["PrinterConfigurations:Windows:PrinterName"];
                printer = printerFactory.CreatePrinterInstance(Mode.Windows, windowsConfig!);
                break;
            default:
                Console.WriteLine("Invalid parameter. Please use 'tcp', 'ws', or 'windows'.");
                break;
        }

        return printer;
    }

    private static async Task PrintContent(IPrinter printer, string content)
    {
        await printer.Print(content);
        Console.WriteLine("Print job sent successfully.");
    }
}
