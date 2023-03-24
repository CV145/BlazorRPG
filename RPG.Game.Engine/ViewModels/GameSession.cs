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
        Monster? CurrentMonster { get; }

        bool HasMonster { get; }
        MovementUnit Movement { get; }
        void OnLocationChanged(Location newLocation);
    }

    //Current game state/instance
    public class GameSession : IGameSession
    {
        private readonly World _currentWorld;
        public Player CurrentPlayer { get; private set; }
        public Location CurrentLocation { get; private set; }
        public Monster? CurrentMonster { get; private set; }

        public bool HasMonster => CurrentMonster != null;
        public MovementUnit Movement { get; private set; }

        public GameSession()
        {
			this.CurrentPlayer = new Player
			{
				Name = "Flynn",
				CharacterClass = "Samurai",
				CurrentHitPoints = 10,
                MaximumHitPoints = 10,
				Gold = 1000,
				ExperiencePoints = 0,
				Level = 1
			};

            this._currentWorld = WorldFactory.CreateWorld();
            this.Movement = new MovementUnit(this._currentWorld);
            this.CurrentLocation = this.Movement.CurrentLocation;
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(1001));
        }

        public void OnLocationChanged(Location newLocation) =>
            this.CurrentLocation = newLocation;

        public void AddXP()
        {
            this.CurrentPlayer.ExperiencePoints += 10;
        }

        private void GetMonsterAtCurrentLocation() =>
            CurrentMonster = CurrentLocation.HasMonster() ? CurrentLocation.GetMonster() : null;
    }
}
