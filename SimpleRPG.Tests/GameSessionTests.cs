using RPG.Game.Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRPG.Tests
{
    public class GameSessionTests
    {
        [Fact]
        public void CreateGameSession()
        {
            // arrange

            // act
            var vm = new GameSession();

            // assert
            Assert.NotNull(vm);
            Assert.NotNull(vm.CurrentPlayer);
            Assert.Equal("DarthPedro", vm.CurrentPlayer.Name);
            Assert.Equal("Fighter", vm.CurrentPlayer.CharacterClass);
            Assert.Equal(10, vm.CurrentPlayer.HitPoints);
            Assert.Equal(1000, vm.CurrentPlayer.Gold);
            Assert.Equal(0, vm.CurrentPlayer.ExperiencePoints);
            Assert.Equal(1, vm.CurrentPlayer.Level);
        }

        [Fact]
        public void AddXP()
        {
            // arrange
            var vm = new GameSession();

            // act
            vm.AddXP();

            // assert
            Assert.Equal(10, vm.CurrentPlayer.ExperiencePoints);
        }
    }
}
