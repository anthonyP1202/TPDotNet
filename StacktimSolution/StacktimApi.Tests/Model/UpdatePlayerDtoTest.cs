using Microsoft.EntityFrameworkCore;
using StacktimApi.Data;
using StacktimApi.DTOs;
using StacktimApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacktimApi.Tests.Model
{
    public class UpdatePlayerDtoTest
    {
        private readonly DbContextOptions<StacktimDbContext> _options;
        public UpdatePlayerDtoTest()
        {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            _options = builder.Options;
        }

        [Fact]
        public void TestUpdateDto()
        {
            //A
            UpdatePlayerDTO upDto = new UpdatePlayerDTO();

            //A
            upDto.Email = "coverage";
            upDto.Pseudo = "also";
            upDto.Rank = Ranks.Gold;
            upDto.TotalScore = 0;
            ICollection<Team> teams = [];
            upDto.Teams = teams;

            var email = upDto.Email;
            var pseudo = upDto.Pseudo;
            var rank = upDto.Rank;
            var totalScore = upDto.TotalScore;
            var teamsList = upDto.Teams;

            //A
            Assert.Empty(teamsList);
            Assert.Equal("coverage", email);
            Assert.Equal(rank, Ranks.Gold);
            Assert.Equal(totalScore, 0);
            Assert.Equal("also", pseudo);

        }
    }
}
