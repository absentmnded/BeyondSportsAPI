using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeyondSportsAPI.Data;
using BeyondSportsAPI.Models;
using System.Numerics;

namespace BeyondSportsAPI.Controllers
{
    [Route("BeyondSports/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly BeyondSportsDbContext _context;

        public TeamsController(BeyondSportsDbContext context)
        {
            _context = context;
        }

        // GET: BeyondSports/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _context.Teams.ToListAsync();
            foreach (var team in teams)
            {
                var players = _context.Players.Where(p => p.IdTeam == team.Id).ToList();
                team.Players = players;
            }
            return await _context.Teams.ToListAsync();
        }

        // GET: BeyondSports/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(long id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("There is no team with the provided id.");
            }
            team.Players = await _context.Players.Where(p => p.IdTeam == id).ToListAsync();
            return team;
        }

        // PUT: BeyondSports/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(long id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest("The id of the team in the URL and JSON string must match.");
            }
            if (!TeamExists(id))
            {
                return NotFound("The team with the provided id is not found in the database.");
            }
            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Changes saved.");
        }

        // POST: BeyondSports/Teams
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(TeamBase team)
        {
            _context.Teams.Add(new Team(team));
            await _context.SaveChangesAsync();
            var addedTeam = _context.Teams.OrderBy(t => t.Id).Last();
            return CreatedAtAction("GetTeam", new { id = addedTeam.Id }, addedTeam);
        }

        // DELETE: BeyondSports/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(long id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("There is no team with the provided id.");
            }
            var players = await _context.Players.Where(p => p.IdTeam == id).ToListAsync();
            players.ForEach(p => p.IdTeam = null);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return Ok("The team has been deleted.");
        }

        private bool TeamExists(long id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
