using Microsoft.EntityFrameworkCore;
using StacktimApi.Data;
using StacktimApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacktimApi.Tests.Model
{
    public class CreatePlayerDtoTest
    {
        private readonly DbContextOptions<StacktimDbContext> _options;
        public CreatePlayerDtoTest()
        {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            _options = builder.Options;
        }

        [Fact]
        public void TestCreatePlayerDto()
        {
            //A
            CreatePlayerDTO createPlayerDTO = new CreatePlayerDTO();

            //a
            createPlayerDTO.Email = "mail";
            createPlayerDTO.Pseudo = "pseudo";
            createPlayerDTO.Rank = Ranks.Gold;

            var email = createPlayerDTO.Email; 
            var pseudo = createPlayerDTO.Pseudo;
            var rank = createPlayerDTO.Rank;

            //a


            
        }
    }
}
