using RPG.Game.Engine.Factories;
using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//View <-> ViewModel <-> Model

namespace RPG.Game.Engine.ViewModels
{
    public interface IGameSession
    {
        Player CurrentPlayer { get; }
        Location CurrentLocation { get; }
        void AddXP();
    }

    //Current game state/instance
    public class GameSession : IGameSession
    {
        public World CurrentWorld { get;private set; }
        public Player CurrentPlayer { get; private set; }
        public Location CurrentLocation { get; private set; }

        public GameSession()
        {
			this.CurrentPlayer = new Player
			{
				Name = "Flynn",
				CharacterClass = "Samurai",
				HitPoints = 10,
				Gold = 1000,
				ExperiencePoints = 0,
				Level = 1
			};

            this.CurrentWorld = WorldFactory.CreateWorld();
            this.CurrentLocation = this.CurrentWorld.GetHomeLocation();
        }

        public void AddXP()
        {
            this.CurrentPlayer.ExperiencePoints += 10;
        }
    }
}
