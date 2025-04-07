using MediatR;

namespace WinFormsDemo.Features.GetStatus;

public record GetStatusCommand : IRequest;

public class GetStatusCommandHandler : IRequestHandler<GetStatusCommand>
{
    public GetStatusCommandHandler() { }

    public async Task Handle(GetStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}