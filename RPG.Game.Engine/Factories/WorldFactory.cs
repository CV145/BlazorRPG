using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            var locations = new List<Location>
            {
                new Location
                {
                    XCoordinate = -2,
                    YCoordinate = -1,
                    Name = "Farmer's Field",
                    Description = "There are rows of corn growing here, with giant rats hiding between them.",
                    ImageName = "/images/locations/FarmFields.jpg"
                },
                new Location
                {
                    XCoordinate = -1,
                    YCoordinate = -1,
                    Name = "Farmer's House",
                    Description = "This is the house of your neighbor, Farmer Ted.",
                    ImageName = "/images/locations/Farmhouse.jpg",
                    TraderHere = TraderFactory.GetTraderById(102)
                },
                new Location
                {
                    XCoordinate = 0,
                    YCoordinate = -1,
                    Name = "Home",
                    Description = "This is your home.",
                    ImageName = "/images/locations/Home.jpg"
                },
                new Location
                {
                    XCoordinate = -1,
                    YCoordinate = 0,
                    Name = "Trading Shop",
                    Description = "The shop of Susan, the trader.",
                    ImageName = "/images/locations/Trader.jpg",
                    TraderHere = TraderFactory.GetTraderById(101)
                },
                new Location
                {
                    XCoordinate = 0,
                    YCoordinate = 0,
                    Name = "Town Square",
                    Description = "You see a fountain here.",
                    ImageName = "/images/locations/TownSquare.jpg"
                },
                new Location
                {
                    XCoordinate = 1,
                    YCoordinate = 0,
                    Name = "Town Gate",
                    Description = "There is a gate here, protecting the town from giant spiders.",
                    ImageName = "/images/locations/TownGate.jpg"
                },
                new Location
                {
                    XCoordinate = 2,
                    YCoordinate = 0,
                    Name = "Spider Forest",
                    Description = "The trees in this forest are covered with spider webs.",
                    ImageName = "/images/locations/SpiderForest.jpg"
                },
                new Location
                {
                    XCoordinate = 0,
                    YCoordinate = 1,
                    Name = "Herbalist's Hut",
                    Description = "You see a small hut, with plants drying from the roof.",
                    ImageName = "/images/locations/HerbalistsHut.jpg",
                    QuestsAvailableHere = new List<Quest> { QuestFactory.GetQuestById(1) },
                    TraderHere = TraderFactory.GetTraderById(103)
                },
                new Location
                {
                    XCoordinate = 0,
                    YCoordinate = 2,
                    Name = "Herbalist's Garden",
                    Description = "There are many plants here, with snakes hiding behind them.",
                    ImageName = "/images/locations/HerbalistsGarden.jpg"
                },
            };

            var newWorld = new World(locations);

            // add monsters at their particular location.
            newWorld.LocationAt(-2, -1).AddMonsterEncounter(2, 100);
            newWorld.LocationAt(2, 0).AddMonsterEncounter(3, 100);
            newWorld.LocationAt(0, 2).AddMonsterEncounter(1, 100);

            return newWorld;
        }
    }
}
