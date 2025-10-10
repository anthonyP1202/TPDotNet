using Microsoft.AspNetCore.Mvc;
using StacktimApi.Data;
using StacktimApi.DTOs;
using StacktimApi.Model;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StacktimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly StacktimDbContext _context;

        public PlayerController(StacktimDbContext context)
        {
            _context = context;
        }

        // GET: api/<PlayerController>
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            IEnumerable<Player> player = _context.Players.ToList();
            return player;
        }

        // GET api/<PlayerController>/5
        [HttpGet("{id}")]
        public ActionResult<Player> Get(int id)
        {
            Player player = _context.Players.FirstOrDefault(play=>play.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            return player;
        }

        // POST api/<PlayerController>
        [HttpPost]
        public void Post([FromBody] Player value)
        {
            Player player = new Player();
            player.Email = value.Email;
            player.Teams = value.Teams;
            player.Rank = value.Rank;
            player.Pseudo = value.Pseudo;
            _context.Players.Add(player);
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public ActionResult<Player> Put(int id, [FromBody] Player value)
        {
            Player player = _context.Players.FirstOrDefault(play => play.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            player.Email = value.Email;
            player.Rank = value.Rank;
            player.Pseudo = value.Pseudo;
            
            ICollection<Team> teams = player.Teams.ToList();
            foreach (Team team in value.Teams.ToList())
            {
                if (teams.Contains(team)){
                    continue;
                } else
                {
                    teams.Add(team);
                }
            }
            player.Teams = teams;

            _context.Players.Update(player);
            return player;
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public ActionResult<Player> Delete(int id)
        {
            Player player = _context.Players.FirstOrDefault(play => play.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            _context.Players.Remove(player);
            return player;
        }
    }
}
