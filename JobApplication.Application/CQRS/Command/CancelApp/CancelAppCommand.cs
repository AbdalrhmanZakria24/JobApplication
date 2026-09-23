using MediatR;

namespace JobApplication.Application.CQRS.Command.CancelApp
{
    public record CancelAppCommand(int Id, int candidateId) : IRequest<int>;
}
