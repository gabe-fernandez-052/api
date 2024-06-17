using CQRS.Data;
using MediatR;

namespace CQRS.Queries
{
    public record GetPlayerQuery(int Id) : IRequest<Player?>
    {
    }
}