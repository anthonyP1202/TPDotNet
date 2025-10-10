using Microsoft.AspNetCore.Mvc;
using StacktimApi.Data;
using StacktimApi.DTOs;
using StacktimApi.Model;
using System;
using System.Collections.Immutable;

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
        public Player Get(int id)
        {
            Player player = _context.Players.FirstOrDefault(play=>play.Id == id);
            return player;
        }

        // POST api/<PlayerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
