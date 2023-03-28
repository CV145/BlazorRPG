using RPG.Game.Engine.Factories;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Location
    {
        public Location(int x, int y, string name, string description, string image, Trader? trader = null)
        {
            XCoordinate = x;
            YCoordinate = y;
            Name = name;
            Description = description;
            ImageName = image;
            TraderHere = trader;
        }
        public int XCoordinate { get; }

        public int YCoordinate { get; }

        public string Name { get; } = string.Empty;

        public string Description { get; } = string.Empty;

        public string ImageName { get; } = string.Empty;

        public IList<Quest> QuestsAvailableHere { get; } = new List<Quest>();

        public IList<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();


        public Trader? TraderHere { get; set; } = null;

        public bool HasTrader => TraderHere != null;

        public void AddMonsterEncounter(int monsterId, int chanceOfEncountering)
        {
            if (MonstersHere.Any(m => m.MonsterId == monsterId))
            {
                // this monster has already been added to this location.
                // so overwrite the ChanceOfEncountering with the new number.
                MonstersHere.First(m => m.MonsterId == monsterId)
                            .ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // this monster is not already at this location, so add it.
                MonstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
            }
        }

        public bool HasMonster() => MonstersHere.Any();

        public Monster GetMonster()
        {
            if (HasMonster() == false)
            {
                throw new InvalidOperationException();
            }

            // total the percentages of all monsters at this location.
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

            // Select a random number between 1 and the total (in case the total chances is not 100).
            var result = DiceService.RollD(totalChances);

            // loop through the monster list, 
            // adding the monster's percentage chance of appearing to the runningTotal variable.
            // when the random number is lower than the runningTotal, that is the monster to return.
            int runningTotal = 0;

            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;

                if (result <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
                }
            }

            // If there was a problem, return the last monster in the list.
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
        }
    }
}
