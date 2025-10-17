using IdentityModel.OidcClient;
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
    public class PlayersControllerTests
    {

        private readonly DbContextOptions<StacktimDbContext> _options;
        public PlayersControllerTests() {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            _options = builder.Options;
        }

        [Fact]
        public void GetPlayers_ReturnsAllPlayers()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Arrange
                IEnumerable<Player> users = context.Players.ToList();
                PlayerController playerController = new PlayerController(context);

                //Act
                IEnumerable<PlayerDTO> usersResponse = playerController.Get();

                //Assert
                Assert.Equal(users.Count(), usersResponse.Count());
            }
        }

        [Fact]
        public void GetPlayer_WithValidId_ReturnsPlayer()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Arrange
                PlayerController playerController = new PlayerController(context);
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

                //Act
                ActionResult<PlayerDTO> playerToTest = playerController.Get(player.Id);

                //Test
                var okResult = Assert.IsType<OkObjectResult>(playerToTest.Result);
                PlayerDTO playerDto = Assert.IsType<PlayerDTO>(okResult.Value);
                Assert.Equal(player.Id, playerDto.Id);
            }
        }
        [Fact]
        public void GetPlayer_WithInvalidId_ReturnsNotFound()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Arrange
                PlayerController playerController = new PlayerController(context);
                Player player = context.Players.FirstOrDefault(play=>play.Id == 0);
                if (player != null) {
                    Assert.Fail("how tf d'you get a index 0"); // proly bad fix if you have time future me 
                }

                //act 
                ActionResult<PlayerDTO> playerToTest = playerController.Get(0);

                //TEST 
                Assert.IsType<NotFoundResult>(playerToTest.Result);

            }
        }
    }
}
