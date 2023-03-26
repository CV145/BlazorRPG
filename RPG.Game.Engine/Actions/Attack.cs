using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Actions
{
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

            int damage = DiceService.rollD(_damageDice);
            string message;

            if (damage == 0)
            {
                message = $"{actorName} missed {targetName}.";
            }
            else
            {
                target.TakeDamage(damage);
                message = $"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.";
            }

            return new MessageBox(title, message);
        }
    }
}
