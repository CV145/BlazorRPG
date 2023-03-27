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
        void ConsumeCurrentItem(GameItem? item);
        void CraftItem(Recipe recipe);
        void ProcessKeyPress(KeyProcessingEventArgs args);
        void AddDisplayMessage(MessageBox message);
    }

    //Current game state/instance can be used inside Razor views
    public class GameSession : IGameSession
    {
        private readonly World _currentWorld;
		private readonly int _maximumMessagesCount = 100;
        private readonly Dictionary<string, Action> _userInputActions = new Dictionary<string, Action>();

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
            InitializeUserInputActions();

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

            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.GetRecipeById(1));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3003));
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

        public void ConsumeCurrentItem(GameItem? item)
        {
            if (item is null || item.Category != GameItem.ItemCategory.Consumable)
            {
                AddDisplayMessage("Item Warning", "You must select a consumable item to use.");
                return;
            }

            // player uses consumable item to heal themselves and item is removed from inventory.
            CurrentPlayer.CurrentConsumable = item;
            var message = CurrentPlayer.UseCurrentConsumable(CurrentPlayer);
            AddDisplayMessage(message);
        }

        public void CraftItem(Recipe recipe)
        {
            _ = recipe ?? throw new ArgumentNullException(nameof(recipe));

            var lines = new List<string>();

            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.Inventory.RemoveItems(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.Inventory.AddItem(outputItem);
                        lines.Add($"You craft 1 {outputItem.Name}");
                    }
                }

                AddDisplayMessage("Item Creation", lines);
            }
            else
            {
                lines.Add("You do not have the required ingredients:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    lines.Add($"  {itemQuantity.Quantity} {ItemFactory.GetItemName(itemQuantity.ItemID)}");
                }

                AddDisplayMessage("Item Creation", lines);
            }
        }

        public void ProcessKeyPress(KeyProcessingEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var key = args.Key.ToUpper();
            if (_userInputActions.ContainsKey(key))
            {
                _userInputActions[key].Invoke();
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
                MessageBoxBroker.Instance.RaiseMessage(new MessageBox("Monster Encountered2:", $"You see a {CurrentMonster.Name} here!"));
			}
		}

        private void GetQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    AddDisplayMessage(quest.ToDisplayMessage());
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
                        CurrentPlayer.Inventory.RemoveItems(quest.ItemsToComplete);

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

        private void InitializeUserInputActions()
        {
            _userInputActions.Add("W", () => Movement.MoveNorth());
            _userInputActions.Add("A", () => Movement.MoveWest());
            _userInputActions.Add("S", () => Movement.MoveSouth());
            _userInputActions.Add("D", () => Movement.MoveEast());
            _userInputActions.Add("ARROWUP", () => Movement.MoveNorth());
            _userInputActions.Add("ARROWLEFT", () => Movement.MoveWest());
            _userInputActions.Add("ARROWDOWN", () => Movement.MoveSouth());
            _userInputActions.Add("ARROWRIGHT", () => Movement.MoveEast());
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

        public void AddDisplayMessage(MessageBox message)
        {
            this.Messages.Insert(0, message);

            if (Messages.Count > _maximumMessagesCount)
            {
                Messages.Remove(Messages.Last());
            }
        }
    }
}
