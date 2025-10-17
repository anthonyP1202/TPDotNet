using Microsoft.EntityFrameworkCore;
using StacktimApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacktimApi.Tests.Helpers
{
    public class TestDbContextFactory
    {
        [Fact]
        public void createTestUser()
        {
            DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext> builder = new DbContextOptionsBuilder<StacktimApi.Data.StacktimDbContext>();
            builder.UseInMemoryDatabase("StacktimDb");
            DbContextOptions<StacktimApi.Data.StacktimDbContext> option = builder.Options;

            using (StacktimApi.Data.StacktimDbContext context = new StacktimApi.Data.StacktimDbContext(option))
            {
                List<Player> players = new List<Player>
                {
                    new Player { Email = "testting@test.com", Pseudo = "testting", Rank = "Gold", TotalScore = 0 },
                    new Player { Email = "outofidea'sbrother@test.com", Pseudo = "outofidea'sbrother", Rank = "Silver", TotalScore = 0 }
                };

                context.AddRange(players);
                context.SaveChanges();
            }
        }
    }
}
