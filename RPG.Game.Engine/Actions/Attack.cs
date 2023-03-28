using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Actions
{
    //ArmorClass + Dexterity roll against opponent attack value
    public class Attack : IAction
    {
        private readonly GameItem _itemInUse;
        private readonly int _damageDice;

        public Attack(GameItem itemInUse, int damageDice)
        {
            _itemInUse = itemInUse ?? throw new ArgumentNullException(nameof(itemInUse));

            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            _damageDice = damageDice;
        }

        public MessageBox Execute(LivingEntity actor, LivingEntity target)
        {
            _ = actor ?? throw new ArgumentNullException(nameof(actor));
            _ = target ?? throw new ArgumentNullException(nameof(target));

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";
            string title = (actor is Player) ? "Player Combat" : "Monster Combat";
			string message;

            if (AttackSucceeded(actor, target))
            {
                int damage = DiceService.RollD(_damageDice);
                target.TakeDamage(damage);

                message = $"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.";
            }
            else
            {
                message = $"{actorName} missed {targetName}.";
            }

            return new MessageBox(title, message);
        }

		private bool AttackSucceeded(LivingEntity actor, LivingEntity target)
		{
			int actorBonus = AbilityCalculator.CalculateBonus(actor.Strength);
			int actorAttack = DiceService.RollD(20) + actorBonus + actor.Level;
			int targetAC = target.ArmorClass + AbilityCalculator.CalculateBonus(target.Dexterity);

			return actorAttack >= targetAC;
		}
	}
}
