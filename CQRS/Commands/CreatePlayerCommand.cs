using MediatR;

namespace CQRS.Commands
{
    public record CreatePlayerCommand(string Name, int Level) : IRequest<int>
    {
    }
}