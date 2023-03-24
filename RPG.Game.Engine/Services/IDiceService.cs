using D20Tek.DiceNotation.DieRoller;
using D20Tek.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Services
{
    public interface IDiceService
    {
        public enum RollerType
        {
            Random = 0,
            Crypto = 1,
            MathNet = 2
        }

        IDice Dice { get; }

        IDiceConfiguration Configuration { get; }

        IDieRollTracker? RollTracker { get; }

        void Configure(RollerType rollerType, bool enableTracker = false);

        DiceResult Roll();

        DiceResult Roll(string diceNotation);
    }
}
