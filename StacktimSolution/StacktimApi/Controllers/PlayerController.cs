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
            return convertToDTO(player);
        }

        // POST api/<PlayerController>
        [HttpPost]
        public ActionResult<PlayerDTO> Post([FromBody] Player value)
        {
            Player player = new Player();
            player.Email = value.Email;
            player.Teams = value.Teams;
            player.Rank = value.Rank;
            player.Pseudo = value.Pseudo;
            try
            {
                _context.Players.Add(player);
                _context.SaveChanges();
            } catch (Exception err)
            {
                Console.WriteLine(err);
                return NotFound();
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

            _context.Players.Update(player);
            _context.SaveChanges();
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
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("leaderboard")]
        public ActionResult<IEnumerable<PlayerDTO>> LeaderBoard()
        {
            IEnumerable<Player> players = _context.Players.FromSqlRaw("SELECT TOP 10 * FROM Players ORDER BY \"TotalScore\" desc ").ToList();
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
