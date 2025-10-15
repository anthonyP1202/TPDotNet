using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StacktimApi.Data;
using StacktimApi.DTOs;
using StacktimApi.Model;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StacktimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class 
        TeamController : ControllerBase
    {
        private readonly StacktimDbContext _context;

        public TeamController(StacktimDbContext context)
        {
            _context = context;
        }

        // GET: api/<TeamController>
        [HttpGet]
        public IEnumerable<Team> Get()
        {
            IEnumerable<Team> teams = _context.Teams.ToList();
            return teams;
        }

        // GET api/<TeamController>/5
        [HttpGet("{id}")]
        public ActionResult<Team> Get(int id)
        {
            Team team = _context.Teams.FirstOrDefault(tem => tem.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            return (team);
        }

        // POST api/<TeamController>
        [HttpPost]
        public ActionResult<Team> Post([FromBody] Team value)
        {
            Team team = new Team();

            team.Name = value.Name;
            team.CaptainId = value.CaptainId;
            team.Tag = value.Tag;

            try
            {
                _context.Teams.Add(team);
                _context.SaveChanges();
            }
            catch (Exception err)
            {
                if (err.InnerException is System.Runtime.InteropServices.ExternalException comEx)
                {
                    if (comEx.ErrorCode == -2146232060)
                    {
                        Console.WriteLine(err);
                        return BadRequest();
                    }
                }
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{id}/raoster")]
        public ActionResult<IEnumerable<Player>> Roster(int id)
        {
            IEnumerable<TeamPlayer> teamPlayers = _context.TeamPlayers.Where(tem => tem.TeamId == id).ToList();
            
            if (teamPlayers == null)
            {
                return NotFound();
            }
            List<Player> players = new List<Player> { };
            foreach (TeamPlayer teamPlayer in teamPlayers)
            {
                Player p = _context.Players.FirstOrDefault(play => play.Id == teamPlayer.PlayerId);
                players.Add(p);
            }
            return (Ok(players));
        }
    }
}
