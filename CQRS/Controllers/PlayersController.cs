using CQRS.Commands;
using CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreatePlayer(CreatePlayerCommand command)
        {
            var id = await sender.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetPlayer(int id)
        {
            var player = await sender.Send(new GetPlayerQuery(id));

            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }
    }
}