## Example Usage	
### Simple hello world example
    var content = "^XA^FO50,50^ADN,36,20^FDHello, World!^FS^XZ";
    var printerFactory = new PrinterFactory();
    IPrinter printer = null;

    // TCP mode
    printer = printerFactory.CreatePrinterInstance(Mode.Tcp, "192.168.1.100", 9100);
    await printer.Print(content);
	
	// WebSocket mode
    printer = printerFactory.CreatePrinterInstance(Mode.WebSocket, "ws://192.168.1.100:12345");
    await printer.Print(content);
    
	// Windows network mode
    printer = printerFactory.CreatePrinterInstance(Mode.Windows, "PutPrinterNameHere");
    await printer.Print(content);
### Example with MediatR
    using MediatR;
	using System.Threading;
	using System.Threading.Tasks;

	public record PrintCommand(string Content) : IRequest;

	public class PrintCommandHandler : IRequestHandler<PrintCommand>
	{
	    private readonly IPrinterFactory printerFactory;

	    public PrintCommandHandler(IPrinterFactory printerFactory) =>
		    this.printerFactory = printerFactory;

	    public async Task<Unit> Handle(PrintCommand request, CancellationToken cancellationToken)
	    {
	        var content = request.Content;

	        // TCP mode
	        var printer = printerFactory.CreatePrinterInstance(Mode.Tcp, "192.168.1.100", 9100);
	        await printer.Print(content);

	        // WebSocket mode
	        printer = printerFactory.CreatePrinterInstance(Mode.WebSocket, "ws://192.168.1.100:12345");
	        await printer.Print(content);

	        // Windows network mode
	        printer = printerFactory.CreatePrinterInstance(Mode.Windows, "PutPrinterNameHere");
	        await printer.Print(content);

	        return Unit.Value;
	    }
	}
}
