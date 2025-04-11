namespace POS.Printers;

public interface IPrinterFactory
{
    IPrinter CreatePrinterInstance(Mode mode, string identifier, int? port = 9100);
}
