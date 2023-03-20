using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//View <-> ViewModel <-> Model

namespace RPG.Game.Engine.ViewModels
{
    public class GameSession
    {
        public Player CurrentPlayer { get; set; }

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
		}

        public void AddXP()
        {
            this.CurrentPlayer.ExperiencePoints += 10;
        }
    }
}
