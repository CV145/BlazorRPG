using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Actions
{
    public interface IAction
    {
        MessageBox Execute(LivingEntity actor, LivingEntity target);
    }
}
