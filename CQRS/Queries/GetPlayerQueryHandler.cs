using CQRS.Data;
using MediatR;

namespace CQRS.Queries
{
    public class GetPlayerQueryHandler(Data.DbContext dbContext) : IRequestHandler<GetPlayerQuery, Player?>
    {
        public async Task<Player?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Players.FindAsync(request.Id);
        }
    }
}