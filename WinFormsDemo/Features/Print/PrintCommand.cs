using MediatR;
using Zebra.Printers;

namespace WinFormsDemo.Features.Print;

public record PrintCommand(PrintParams Params) : IRequest;

public class PrintCommandHandler : IRequestHandler<PrintCommand>
{
    private readonly IPrinterFactory factory;

    public PrintCommandHandler(IPrinterFactory factory)
    {
        this.factory = factory;
    }

    public async Task Handle(PrintCommand request, CancellationToken cancellationToken)
    {
        IPrinter? printer = factory.CreatePrinterInstance(
            Mode.Tcp, 
            request.Params.Identifier, 
            request.Params.port);

        await printer.Print(request.Params.Content);
    }
}