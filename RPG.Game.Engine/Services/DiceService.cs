using D20Tek.DiceNotation.DieRoller;
using D20Tek.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Services
{
    /*
     * Offers dice rolling services as a singleton (one instance only)
     */
    public static class DiceService
    {
        public static int RollD(int amount)
        {
            Console.WriteLine("rolling dice");
            Random random = new Random();
            int result = random.Next(1, amount+1);
            Console.WriteLine(result);
            Console.WriteLine("dice rolled");
            return result;
        }
    }
}
