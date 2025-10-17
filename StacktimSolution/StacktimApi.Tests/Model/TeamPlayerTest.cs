using Microsoft.EntityFrameworkCore;
using StacktimApi.Data;
using StacktimApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacktimApi.Tests.Model
{
    public class TeamPlayerTest
    {
        private readonly DbContextOptions<StacktimDbContext> _options;
        public TeamPlayerTest()
        {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            _options = builder.Options;
        }

        [Fact]
        public void TestTeamPlayer()
        {
            //Set
            TeamPlayer teamPlayer = new TeamPlayer();

            //ACt
            teamPlayer.PlayerId = 1;
            teamPlayer.TeamId = 2;
            teamPlayer.Role = 0;
            teamPlayer.JoinDate = DateTime.Now;

            var teamId = teamPlayer.TeamId;
            var playerId = teamPlayer.PlayerId;
            var role = teamPlayer.Role;
            var joinDate = teamPlayer.JoinDate;

            //Test
            Assert.Equal(1, teamPlayer.PlayerId);
            Assert.Equal(2, teamPlayer.TeamId);
            Assert.Equal(0, teamPlayer.Role);
            Assert.Equal(teamPlayer.JoinDate, teamPlayer.JoinDate); //this one is ridiculous even by my standard
        }
    }
}
