using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeyondSportsAPI.Models;
using BeyondSportsAPI.Data;
using Microsoft.IdentityModel.Tokens;

namespace BeyondSportsAPI.Controllers
{
    [Route("BeyondSports/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly BeyondSportsDbContext _context;

        public PlayersController(BeyondSportsDbContext context)
        {
            _context = context;
        }

        // GET: BeyondSports/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: BeyondSports/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound("There is no player with the provided id.");
            }

            return player;
        }

        // PUT: BeyondSports/Players/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest("The id of the player in the URL and JSON string must match.");
            }
            if (!PlayerExists(id))
            {
                return NotFound("The player with the provided id is not found in the database.");
            }
            if ((player.IdTeam != null) && (!TeamExists((long)player.IdTeam)))
                return BadRequest("There is no team with the provided id.");

            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Changes saved.");
        }

        // PUT: BeyondSports/Players/AddToTeam/5
        [HttpPut]
        [Route("AddToTeam/{id}")]
        public async Task<IActionResult> PutAddPlayersToTeam(long id, PlayersOnTheTeam assignedPlayers)
        {
            if (id != assignedPlayers.IdTeam)
            {
                return BadRequest("The id of the team in the URL and JSON string must match.");
            }
            if (!TeamExists(id))
            {
                return NotFound("The team with the provided id is not found in the database.");
            }
            if (!assignedPlayers.IdsPlayer.IsNullOrEmpty())
            {
                var message = "";
                var players = await _context.Players.Where(p => assignedPlayers.IdsPlayer!.Contains(p.Id)).ToListAsync();
                if (players.Count > 0)
                {
                    players.ForEach(p => p.IdTeam = assignedPlayers.IdTeam);
                    message += "The  players with Ids: " + string.Join(", ",players.Select(p => p.Id.ToString())) + " have been added to the team with Id " + assignedPlayers.IdTeam + ".";
                    await _context.SaveChangesAsync();
                    var missingPlayers = assignedPlayers.IdsPlayer!.Except(players.Select(p => p.Id).ToArray()).ToList();
                    message += (missingPlayers.Count > 0) ? " The players with the following ids: " + string.Join(", ", missingPlayers) + " are missing in the database." : "";
                    return Ok(message);
                }
                else
                {
                    return Ok("No players in the database has any of the provided ids.");
                }
            }
            else
            {
                return BadRequest("No player ids have been provided");
            }
        }

        // PUT: BeyondSports/Players/AddToTeam/5
        [HttpPut]
        [Route("RemoveFromTeam")]
        public async Task<IActionResult> PutRemovePlayersFromTeam(long[] playerIds)
        {
            if (playerIds.IsNullOrEmpty())
            {
                return NotFound("No player ids have been provided.");
            }
            var message = "";
            var players = await _context.Players.Where(p => playerIds.Contains(p.Id)).ToListAsync();
            if (players.Count > 0)
            {
                players.ForEach(p => p.IdTeam = null);
                message += "The  players with Ids: " + string.Join(", ", players.Select(p => p.Id.ToString())) + " have been removed from their teams.";
                await _context.SaveChangesAsync();
                var missingPlayers = playerIds.Except(players.Select(p => p.Id).ToArray()).ToList();
                message += (missingPlayers.Count > 0) ? " The players with the following ids: " + string.Join(", ", missingPlayers) + " are missing in the database." : "";
                return Ok(message);
            }
            else
            {
                return Ok("No players in the database has any of the provided ids.");
            }
        }

        // POST: BeyondSports/Players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(PlayerBase player)
        {
            _context.Players.Add(new Player(player));
            await _context.SaveChangesAsync();
            var addedPlayer = _context.Players.OrderBy(p => p.Id).Last();

            return CreatedAtAction("GetPlayer", new { id = addedPlayer.Id }, addedPlayer);
        }

        // DELETE: BeyondSports/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound("There is no player with the provided id.");
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok("The player has been deleted.");
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
        private bool TeamExists(long id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
