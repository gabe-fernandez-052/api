using CQRS.Data;
using MediatR;

namespace CQRS.Commands
{
    public record CreatePlayerCommandHandler(DbContext dbContext) : IRequestHandler<CreatePlayerCommand, int>
    {
        public async Task<int> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var player = new Player()
            {
                Name = request.Name,
                Level = request.Level
            };
            dbContext.Players.Add(player);

            await dbContext.SaveChangesAsync();

            return player.Id;
        }
    }
}