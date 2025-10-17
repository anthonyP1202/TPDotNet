using FluentAssertions;
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

        [Fact]
        public void CreatePlayer_WithValidData_ReturnsCreated()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Setuop
                PlayerController playerController = new PlayerController(context);
                Player playerByEmail = context.Players.FirstOrDefault(p => p.Email == "jetestdestruk");
                Player playerByPseudo = context.Players.FirstOrDefault(p => p.Pseudo == "jtestdestrul");

                if (playerByEmail != null)
                {
                    context.Players.Remove(playerByEmail);
                }

                if (playerByPseudo != null)
                {
                    context.Players.Remove(playerByPseudo);
                } 

                Player player = new Player{
                    Email = "jetestdestruk",
                    Pseudo = "jtestdestrul",
                    Rank = "Gold"
                };

                //Act
                ActionResult<PlayerDTO> succeded = playerController.Post(player);

                //TEST
                Assert.IsType<OkResult>(succeded.Result);
                playerByEmail = context.Players.FirstOrDefault(p => p.Email == "jetestdestruk");
                Assert.True(playerByEmail != null);
                
            }
        }

        [Fact]
        public void CreatePlayer_WithDuplicatePseudo_ReturnsBadRequest()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Set
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
                Player newPlayer = new Player
                {
                    Email = "idon'tcare",
                    Pseudo = player.Pseudo,
                    Rank = "Gold"
                };          

                //ACT 
                ActionResult<PlayerDTO> succeded = playerController.Post(newPlayer);

                //Test
                Assert.IsType<BadRequestResult>(succeded.Result);
                IEnumerable<Player> playersByEmail = context.Players.Where(p => p.Pseudo == player.Pseudo).ToList();
                Assert.True(playersByEmail.Count()==1);
            }
        }

        [Fact]
        public void DeletePlayer_WithValidId_ReturnsNoContent()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //Set
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

                //ACT
                ActionResult<Player> result = playerController.Delete(player.Id);

                //TEST
                Assert.IsType<OkResult>(result.Result);
                Player deletedPlayer = context.Players.FirstOrDefault(play=>play.Id == player.Id);
                Assert.True(deletedPlayer == null);
            }
        }

        [Fact]
        public void GetLeaderboard_ReturnsOrderedPlayers()
        {
            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(_options))
            {
                //SET
                IEnumerable<Player> users = context.Players.ToList();
                PlayerController playerController = new PlayerController(context);

                if (users.Count() == 0) {
                    try { 
                        List<Player> players = new List<Player>
                        {
                            new Player { Email = "testting@test.com", Pseudo = "testting", Rank = "Gold", TotalScore = 20 },
                            new Player { Email = "outofidea'sbrother@test.com", Pseudo = "outofidea'sbrother", Rank = "Silver", TotalScore = 30 },
                            new Player { Email = "alpha@test.com", Pseudo = "Alpha", Rank = "Bronze", TotalScore = 10 },
                            new Player { Email = "bravo@test.com", Pseudo = "Bravo", Rank = "Silver", TotalScore = 20 },
                            new Player { Email = "charlie@test.com", Pseudo = "Charlie", Rank = "Gold", TotalScore = 30 },
                            new Player { Email = "delta@test.com", Pseudo = "Delta", Rank = "Platinum", TotalScore = 40 },
                            new Player { Email = "echo@test.com", Pseudo = "Echo", Rank = "Diamond", TotalScore = 50 },
                            new Player { Email = "foxtrot@test.com", Pseudo = "Foxtrot", Rank = "Master", TotalScore = 60 },
                            new Player { Email = "golf@test.com", Pseudo = "Golf", Rank = "Bronze", TotalScore = 15 },
                            new Player { Email = "hotel@test.com", Pseudo = "Hotel", Rank = "Silver", TotalScore = 25 },
                            new Player { Email = "india@test.com", Pseudo = "India", Rank = "Gold", TotalScore = 35 },
                            new Player { Email = "juliet@test.com", Pseudo = "Juliet", Rank = "Platinum", TotalScore = 45 },
                            new Player { Email = "kilo@test.com", Pseudo = "Kilo", Rank = "Diamond", TotalScore = 55 },
                            new Player { Email = "lima@test.com", Pseudo = "Lima", Rank = "Master", TotalScore = 65 }

                        };
                        context.AddRange(players);
                        context.SaveChanges();
                    } catch
                    {

                    }
                }
                users = context.Players.ToList();

                //ACT
                ActionResult<IEnumerable<PlayerDTO>> response = playerController.LeaderBoard();

                //TEST
                var okResult = Assert.IsType<OkObjectResult>(response.Result);
                IEnumerable<PlayerDTO> playerDtos = Assert.IsAssignableFrom<IEnumerable<PlayerDTO>>(okResult.Value);
                Assert.True(playerDtos.Count() <= 10);

                var ordered = playerDtos.OrderByDescending(p => p.TotalScore).ToList();
                var original = playerDtos.ToList();

                Assert.True(original.SequenceEqual(ordered));
            }
        }
    }
}
