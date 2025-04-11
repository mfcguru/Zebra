namespace POS.Printers;

public sealed class PrinterFactory : IPrinterFactory
{
    public IPrinter CreatePrinterInstance(Mode mode, string identifier, int? port = 9100)
    {
        return mode switch
        {
            Mode.Tcp => new TcpPrinter(identifier, port),
            Mode.WebSocket => new WebSocketPrinter(identifier),
            Mode.Windows => new WindowsPrinter(identifier),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}
