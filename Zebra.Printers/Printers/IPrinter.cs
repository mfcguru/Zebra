namespace POS.Printers;

public interface IPrinter
{
    Task Print(string command);
}

