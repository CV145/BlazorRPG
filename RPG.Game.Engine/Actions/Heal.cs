using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Actions
{
    public class Heal : IAction
    {
        private readonly GameItem _item;
        private readonly int _hitPointsToHeal;

        public Heal(GameItem item, int hitPointsToHeal)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            if (item.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{item.Name} is not consumable");
            }

            if (hitPointsToHeal <= 0)
            {
                throw new ArgumentOutOfRangeException($"{item.Name} must have positive healing value.");
            }

            _hitPointsToHeal = hitPointsToHeal;
        }

        public MessageBox Execute(LivingEntity actor, LivingEntity target)
        {
            _ = actor ?? throw new ArgumentNullException(nameof(actor));
            _ = target ?? throw new ArgumentNullException(nameof(target));

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";

            target.Heal(_hitPointsToHeal);

            return new MessageBox(
                "Heal Effect",
                $"{actorName} heal {targetName} for {_hitPointsToHeal} point{(_hitPointsToHeal > 1 ? "s" : "")}.");
        }
    }
}
