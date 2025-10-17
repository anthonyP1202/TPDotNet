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
    public class PlayerController : ControllerBase
    {
        private readonly StacktimDbContext _context;

        public PlayerController(StacktimDbContext context)
        {
            _context = context;
        }

        // GET: api/<PlayerController>
        [HttpGet]
        public IEnumerable<PlayerDTO> Get()
        {
            IEnumerable<Player> players = _context.Players.ToList();

            List<PlayerDTO> playerDTO = new List<PlayerDTO> { };
            
            foreach(Player player in players)
            {
                playerDTO.Add(convertToDTO(player));
            }
            
            IEnumerable<PlayerDTO> iPlayerDTO = playerDTO;
            return iPlayerDTO;
        }

        // GET api/<PlayerController>/5
        [HttpGet("{id}")]
        public ActionResult<PlayerDTO> Get(int id)
        {
            Player player = _context.Players.FirstOrDefault(play=>play.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(convertToDTO(player));
        }

        // POST api/<PlayerController>
        [HttpPost]
        public ActionResult<PlayerDTO> Post([FromBody] Player value)
        {
            String[] array = new String[] { "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Master" };
            Player playerByEmail = _context.Players.FirstOrDefault(p => p.Email == value.Email);
            Player playerByPseudo = _context.Players.FirstOrDefault(p => p.Pseudo == value.Pseudo);
            if (playerByPseudo != null || playerByEmail != null) {
                return BadRequest();
            }
            Player player = new Player();
            player.Email = value.Email;
            player.Teams = value.Teams;
            if (array.Contains(value.Rank)){
                player.Rank = value.Rank;
            } else
            {
                player.Rank = "Bronze";
            }
            
            player.Pseudo = value.Pseudo;
            IEnumerable<Team> playerTeams = player.Teams.ToList();
            player.Teams.Clear();
            try
            {
                _context.Players.Add(player);
                _context.SaveChanges();
                foreach (Team team in playerTeams)
                {
                    _context.Database.ExecuteSqlRaw("INSERT INTO TeamPlayers (PlayerId, TeamId, Role) VALUES ({0}, {1}, 0)", player.Id, team.Id);
                }
                _context.SaveChanges();
            } catch (Exception err)
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
            player.TotalScore = value.TotalScore;
            IEnumerable<Team> playerTeams = player.Teams.ToList();
            player.Teams.Clear();
            try
            {
                _context.Players.Update(player);
                _context.SaveChanges();
                foreach (Team team in playerTeams)
                {
                    _context.Database.ExecuteSqlRaw("INSERT INTO TeamPlayers (PlayerId, TeamId, Role) VALUES ({0}, {1}, 0)", player.Id, team.Id);
                }
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
            }
            return Ok();
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
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("leaderboard")]
        public ActionResult<IEnumerable<PlayerDTO>> LeaderBoard()
        {
            IEnumerable<Player> players = _context.Players.OrderByDescending(p => p.TotalScore).Take(10).ToList();

            List<PlayerDTO> playerDTO = new List<PlayerDTO> { };

            foreach (Player player in players)
            {
                playerDTO.Add(convertToDTO(player));
            }

            IEnumerable<PlayerDTO> iPlayerDTO = playerDTO;
            return Ok(iPlayerDTO);
        }

        private static PlayerDTO convertToDTO(Player player)
        {
            Ranks playerRanking = 0;
            switch (player.Rank)
            {
                case "Bronze":
                    playerRanking = Ranks.Bronze;
                    break;
                case "Silver":
                    playerRanking = Ranks.Silver;
                    break;
                case "Gold":
                    playerRanking = Ranks.Gold;
                    break;
                case "Platinum":
                    playerRanking = Ranks.Platinum;
                    break;
                case "Diamond":
                    playerRanking = Ranks.Diamond;
                    break;
                case "Master":
                    playerRanking = Ranks.Master;
                    break;
            }

            PlayerDTO playerDTO = new PlayerDTO();
            playerDTO.Id = player.Id;
            playerDTO.Teams = player.Teams.ToList();
            playerDTO.Email = player.Email;
            playerDTO.Rank = playerRanking;
            playerDTO.TotalScore = player.TotalScore;
            playerDTO.Pseudo = player.Pseudo;

            return playerDTO;
        }
    }
}
