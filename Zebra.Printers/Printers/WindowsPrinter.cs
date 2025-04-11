using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.Versioning;

namespace POS.Printers;

public class WindowsPrinter : IPrinter
{
    private readonly string printerName;

    public WindowsPrinter(string printerName)
    {
        this.printerName = printerName;
    }

    [SupportedOSPlatform("windows")]
    public Task Print(string command)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        var printDocument = new PrintDocument
        {
            PrinterSettings = new PrinterSettings
            {
                PrinterName = printerName
            }
        };

        printDocument.PrintPage += (sender, e) =>
        {
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(command));
            using var reader = new StreamReader(stream);
            var zplCommand = reader.ReadToEnd();
            if (e.Graphics != null)
            {
                e.Graphics.DrawString(zplCommand, new Font("Arial", 10), Brushes.Black, new PointF(0, 0));
            }
        };

        printDocument.EndPrint += (sender, e) => taskCompletionSource.SetResult(true);

        printDocument.Print();

        return taskCompletionSource.Task;
    }
}

