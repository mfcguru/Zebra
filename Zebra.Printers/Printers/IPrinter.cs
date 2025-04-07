namespace Zebra.Printers;

public interface IPrinter
{
    Task Print(string command);
}

