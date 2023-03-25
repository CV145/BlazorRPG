using RPG.Game.Engine.Factories;
using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
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
		Trader? CurrentTrader { get; }
        MovementUnit Movement { get; }
        void OnLocationChanged(Location newLocation);
        void AttackCurrentMonster(Weapon? currentWeapon);
    }

    //Current game state/instance
    public class GameSession : IGameSession
    {
        private readonly World _currentWorld;
		private readonly IDiceService _diceService;
		private readonly int _maximumMessagesCount = 100;

		public Player CurrentPlayer { get; private set; }
        public Location CurrentLocation { get; private set; }
        public Monster? CurrentMonster { get; private set; }

        public bool HasMonster => CurrentMonster != null;
        public Trader? CurrentTrader { get; private set; }
        public MovementUnit Movement { get; private set; }

		public IList<MessageBox> Messages { get; } = new List<MessageBox>();

		public GameSession(int maxMessageCount)
			: this()
		{
			_maximumMessagesCount = maxMessageCount;
		}

		public GameSession(IDiceService? diceService = null)
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
            GetMonsterAtCurrentLocation();

			if (!CurrentPlayer.Inventory.Weapons.Any())
			{
				CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(1001));
			}
        }

        public void OnLocationChanged(Location newLocation)
        {
            _ = newLocation ?? throw new ArgumentNullException(nameof(newLocation));

            CurrentLocation = newLocation;
            Movement.UpdateLocation(CurrentLocation);
            GetMonsterAtCurrentLocation();
            CurrentTrader = CurrentLocation.TraderHere;
        }

        public void AttackCurrentMonster(Weapon? currentWeapon)
		{
			Console.WriteLine("Attacking current monster");
			if (CurrentMonster is null)
			{
				AddDisplayMessage("Error", "Current monster is null");
				return;
			}

			if (currentWeapon is null)
			{
				AddDisplayMessage("Combat Warning", "You must select a weapon, to attack.");
				return;
			}

			// Determine damage to monster
			int damageToMonster = _diceService.Roll(currentWeapon.DamageRoll).Value;

			if (damageToMonster == 0)
			{
				AddDisplayMessage("Player Combat", $"You missed the {CurrentMonster.Name}.");
			}
			else
			{
				CurrentMonster.CurrentHitPoints -= damageToMonster;
				AddDisplayMessage("Player Combat", $"You hit the {CurrentMonster.Name} for {damageToMonster} points.");
			}

			// If monster if killed, collect rewards and loot
			if (CurrentMonster.CurrentHitPoints <= 0)
			{
				var messageLines = new List<string>();
				messageLines.Add($"You defeated the {CurrentMonster.Name}!");

				CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
				messageLines.Add($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

				CurrentPlayer.Gold += CurrentMonster.Gold;
				messageLines.Add($"You receive {CurrentMonster.Gold} gold.");

				foreach (GameItem item in CurrentMonster.Inventory.Items)
				{
					CurrentPlayer.Inventory.AddItem(item);
					messageLines.Add($"You received {item.Name}.");
				}

				AddDisplayMessage("Monster Defeated", messageLines);

				// Get another monster to fight
				GetMonsterAtCurrentLocation();
			}
			else
			{
				// If monster is still alive, let the monster attack
				int damageToPlayer = _diceService.Roll(CurrentMonster.DamageRoll).Value;

				if (damageToPlayer == 0)
				{
					AddDisplayMessage("Monster Combat", "The monster attacks, but misses you.");
				}
				else
				{
					CurrentPlayer.CurrentHitPoints -= damageToPlayer;
					AddDisplayMessage("Monster Combat", $"The {CurrentMonster.Name} hit you for {damageToPlayer} points.");
				}

				// If player is killed, move them back to their home.
				if (CurrentPlayer.CurrentHitPoints <= 0)
				{
					AddDisplayMessage("Player Defeated", $"The {CurrentMonster.Name} killed you.");

					CurrentPlayer.CurrentHitPoints = CurrentPlayer.MaximumHitPoints; // Completely heal the player
					this.OnLocationChanged(_currentWorld.LocationAt(0, -1)); // Return to Player's home
				}
			}
		}

		private void GetMonsterAtCurrentLocation()
		{
			CurrentMonster = CurrentLocation.HasMonster() ? CurrentLocation.GetMonster() : null;

			if (CurrentMonster != null)
			{
				AddDisplayMessage("Monster Encountered:", $"You see a {CurrentMonster.Name} here!");
			}
		}

		private void AddDisplayMessage(string title, string message) =>
			AddDisplayMessage(title, new List<string> { message });

		private void AddDisplayMessage(string title, IList<string> messages)
		{
			Console.WriteLine(title + ": " + messages.Count);
			var message = new MessageBox(title, messages);
			this.Messages.Insert(0, message);

			if (Messages.Count > _maximumMessagesCount)
			{
				Messages.Remove(Messages.Last());
			}
		}
	}
}
