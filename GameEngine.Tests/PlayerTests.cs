using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameEngine.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void CreateSimplePlayer()
        {
            // arrange

            // act
            var p = new Player
            {
                Name = "Test",
                Level = 1,
                HitPoints = 10
            };

            // assert
            Assert.NotNull(p);
            Assert.Equal("Test", p.Name);
            Assert.Equal(string.Empty, p.CharacterClass);
            Assert.Equal(1, p.Level);
            Assert.Equal(10, p.HitPoints);
            Assert.Equal(0, p.ExperiencePoints);
            Assert.Equal(0, p.Gold);
        }
    }
}
