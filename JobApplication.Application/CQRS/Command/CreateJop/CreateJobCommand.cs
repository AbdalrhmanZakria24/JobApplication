using MediatR;

namespace JobApplication.Application.CQRS.Command.CreateJop
{
    public record CreateJobCommand(string Title, string Description) : IRequest<int>;
}
