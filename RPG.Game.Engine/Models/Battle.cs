using RPG.Game.Engine.Services;
using RPG.Game.Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
	public class Battle
	{

		public enum Combatant
		{
			Player,
			Opponent
		}

		private readonly MessageBoxBroker _messageBroker = MessageBoxBroker.Instance;
		private readonly Action _onPlayerKilled;
		private readonly Action _onOpponentKilled;

		public Battle(Action onPlayerKilled, Action onOpponentKilled)
		{
			_onPlayerKilled = onPlayerKilled;
			_onOpponentKilled = onOpponentKilled;
		}

		public void Attack(Player player, Monster opponent, GameSession session)
		{
			_ = player ?? throw new ArgumentNullException(nameof(player));
			_ = opponent ?? throw new ArgumentNullException(nameof(opponent));

				bool battleContinues = AttackOpponent(player, opponent, session);
				if (battleContinues)
				{
					// if the monster is still alive, it attacks the player.
					AttackPlayer(player, opponent);
				}
		}

		private bool AttackOpponent(Player player, Monster opponent, GameSession session)
		{
			if (player.CurrentWeapon == null)
			{
				_messageBroker.RaiseMessage(new MessageBox("Combat Warning", "You must select a weapon in order to fight"));
				return false;
			}

			//player attacks monster with weapon
			var message = player.UseCurrentWeaponOn(opponent);
			_messageBroker.RaiseMessage(message);

			//if monster is killed, collect rewards and loot
			if(opponent.IsDead)
			{
				OnOpponentKilled(player, opponent, session);
				return false;
			}

			return true;
		}

		private bool AttackPlayer(Player player, Monster opponent) {
			var message = opponent.UseCurrentWeaponOn(player);
			_messageBroker.RaiseMessage(message);

			//game over
			if (player.IsDead)
			{
				OnPlayerKilled(player, opponent);
				return false;
			}
			return true;
		}

		private void OnPlayerKilled(Player player, Monster opponent)
		{
			_messageBroker.RaiseMessage(
				new MessageBox("Player Defeated", $"The {opponent.Name} killed you."));

			player.CompletelyHeal();  // Completely heal the player.
			_onPlayerKilled.Invoke();  // Action to reset player to home location.
		}


		private void OnOpponentKilled(Player player, Monster opponent, GameSession session)
		{
			var messageLines = new List<string>
			{
				$"You defeated the {opponent.Name}!"
			};

			player.AddExperience(opponent.RewardExperiencePoints);
			messageLines.Add($"You receive {opponent.RewardExperiencePoints} experience points.");

			player.ReceiveGold(opponent.Gold);
			messageLines.Add($"You receive {opponent.Gold} gold.");

			foreach (GameItem item in opponent.Inventory.Items)
			{
				player.Inventory.AddItem(item);
				messageLines.Add($"You received {item.Name}.");
			}

			_messageBroker.RaiseMessage(new MessageBox("Monster Defeated", messageLines));

			//_onOpponentKilled.Invoke();  // Action to get another opponent.

			session.DefeatCurrentMonster();
		}
	}
}
