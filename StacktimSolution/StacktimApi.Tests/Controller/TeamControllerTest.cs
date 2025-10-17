using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StacktimApi.Controllers;
using StacktimApi.Data;
using StacktimApi.DTOs;
using StacktimApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacktimApi.Tests.Controller
{
    public class TeamControllerTest
    {
        private readonly DbContextOptions<StacktimDbContext> _options;
        public TeamControllerTest() {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            _options = builder.Options;
        }

        [Fact]
        public void GetTeams_ReturnsAllTeams()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Set
                TeamController teamController = new TeamController(context);
                IEnumerable<Team> teams = context.Teams.ToList();

                //Act
                IEnumerable<Team> teamsList = teamController.Get();

                //Test
                Assert.Equal(teams.Count(), teamsList.Count());
            }
        }

        [Fact]
        public void GetPlayer_WithValidId_ReturnsPlayer()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Arrange
                TeamController teamController = new TeamController(context);
                Team team = context.Teams.FirstOrDefault();

                Player player = context.Players.FirstOrDefault();
                if (player == null)
                {
                    List<Player> players = new List<Player>
                    {
                        new Player { Email = "testting@test.com", Pseudo = "testting", Rank = "Gold", TotalScore = 0 },
                        new Player { Email = "outofidea'sbrother@test.com", Pseudo = "outofidea'sbrother", Rank = "Silver", TotalScore = 0 }
                    };

                    context.AddRange(players);
                    context.SaveChanges();
                }
                player = context.Players.First();

                if (team == null)
                {
                    List<Team> teams = new List<Team>
                    {
                        new Team { Name = "testting@test.com", CaptainId = player.Id, Tag = "GOL"},
                        new Team { Name = "estse", CaptainId = player.Id, Tag = "PPP" }
                    };

                    context.AddRange(teams);
                    context.SaveChanges();
                }
                team = context.Teams.First();

                //Act
                ActionResult<Team> teamToTest = teamController.Get(team.Id);

                //Test
                var okResult = Assert.IsType<OkObjectResult>(teamToTest.Result);
                Team team2 = Assert.IsType<Team>(okResult.Value);
                Assert.Equal(team.Id, team2.Id);
            }
        }
    }
}
