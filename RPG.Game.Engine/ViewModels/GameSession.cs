using RPG.Game.Engine.Factories;
using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//View <-> ViewModel <-> Model

//Contains game session data for UI

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
        void AttackCurrentMonster(GameItem? currentWeapon);
    }

    //Current game state/instance
    public class GameSession : IGameSession
    {
        private readonly World _currentWorld;
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
			CompleteQuestsAtLocation();
			GetQuestsAtLocation();
            CurrentTrader = CurrentLocation.TraderHere;
        }

        public void AttackCurrentMonster(GameItem? currentWeapon)
		{
			Console.WriteLine("Attacking current monster");
			if (CurrentMonster is null)
			{
				AddDisplayMessage("Error", "Current monster is null");
				return;
			}

			if (currentWeapon is null)
			{
				AddDisplayMessage("Combat Warning", "You must select a weapon to attack.");
				return;
			}

            //act against monster with weapon
            CurrentPlayer.CurrentWeapon = currentWeapon;
			var message = CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);
			AddDisplayMessage(message);

			// If monster if killed, collect rewards and loot
			if (CurrentMonster.IsDead)
			{
                OnCurrentMonsterKilled(CurrentMonster);

				// Get another monster to fight
				GetMonsterAtCurrentLocation();
			}
			else
			{
				// If monster is still alive, let the monster attack
				message = CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
                AddDisplayMessage(message);

				if (CurrentPlayer.IsDead)
				{
                    OnCurrentPlayerKilled(CurrentMonster);
				}
			}
		}

        private void OnCurrentPlayerKilled(Monster currentMonster)
        {
            AddDisplayMessage("Player Defeated", $"The {currentMonster.Name} killed you.");

            CurrentPlayer.CompletelyHeal();  // Completely heal the player
            this.OnLocationChanged(_currentWorld.LocationAt(0, -1));  // Return to Player's home
        }

        private void OnCurrentMonsterKilled(Monster currentMonster)
        {
            var messageLines = new List<string>();
            messageLines.Add($"You defeated the {currentMonster.Name}!");

            CurrentPlayer.AddExperience(currentMonster.RewardExperiencePoints);
            messageLines.Add($"You receive {currentMonster.RewardExperiencePoints} experience points.");

            CurrentPlayer.ReceiveGold(currentMonster.Gold);
            messageLines.Add($"You receive {currentMonster.Gold} gold.");

            foreach (GameItem item in currentMonster.Inventory.Items)
            {
                CurrentPlayer.Inventory.AddItem(item);
                messageLines.Add($"You received {item.Name}.");
            }

            AddDisplayMessage("Monster Defeated", messageLines);
        }


        private void GetMonsterAtCurrentLocation()
		{
			CurrentMonster = CurrentLocation.HasMonster() ? CurrentLocation.GetMonster() : null;

			if (CurrentMonster != null)
			{
				AddDisplayMessage("Monster Encountered:", $"You see a {CurrentMonster.Name} here!");
			}
		}

        private void GetQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    var messageLines = new List<string>
                    {
                        quest.Description,
                        " Items to complete the quest: "
                    };

                    foreach (ItemQuantity q in quest.ItemsToComplete)
                    {
                        messageLines.Add($"{ItemFactory.CreateGameItem(q.ItemID).Name} (x{q.Quantity})");
                    }

                    messageLines.Add("Rewards for quest completion:");
                    messageLines.Add($"   {quest.RewardExperiencePoints} experience points");
                    messageLines.Add($"   {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        messageLines.Add($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name} (x{itemQuantity.Quantity})");
                    }

                    AddDisplayMessage($"Quest Added - {quest.Name}", messageLines);
                }
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.Id == quest.Id &&
                                                             !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Remove the quest completion items from the player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.Inventory.RemoveItem(
                                    CurrentPlayer.Inventory.Items.First(
                                        item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }

                        // give the player the quest rewards
                        var messageLines = new List<string>();
						CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        messageLines.Add($"You receive {quest.RewardExperiencePoints} experience points");

						CurrentPlayer.ReceiveGold(quest.RewardGold);
                        messageLines.Add($"You receive {quest.RewardGold} gold");

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            CurrentPlayer.Inventory.AddItem(rewardItem);
                            messageLines.Add($"You receive a {rewardItem.Name}");
                        }

                        AddDisplayMessage($"Quest Completed - {quest.Name}", messageLines);

                        // mark the quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
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

        private void AddDisplayMessage(MessageBox message)
        {
            this.Messages.Insert(0, message);

            if (Messages.Count > _maximumMessagesCount)
            {
                Messages.Remove(Messages.Last());
            }
        }
    }
}
