using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    internal static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    Monster snake = new Monster
                    {
                        Name = "Snake",
                        ImageName = "/images/monsters/snake.jpg",
                        CurrentHitPoints = 4,
                        MaximumHitPoints = 4,
                        RewardExperiencePoints = 5,
                        Gold = 1,
						Dexterity = 15,
						Strength = 12,
						ArmorClass = 10
					};

                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);
                    return snake;

                case 2:
                    Monster rat = new Monster
                    {
                        Name = "Rat",
                        ImageName = "/images/monsters/rat.jpg",
                        CurrentHitPoints = 5,
                        MaximumHitPoints = 5,
                        RewardExperiencePoints = 5,
                        Gold = 1,
						Dexterity = 8,
						Strength = 10,
						ArmorClass = 10
					};

                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
                    return rat;

                case 3:
                    Monster giantSpider = new Monster
                    {
                        Name = "Giant Spider",
                        ImageName = "/images/monsters/giant-spider.jpg",
                        CurrentHitPoints = 10,
                        MaximumHitPoints = 10,
                        RewardExperiencePoints = 10,
                        Gold = 3,
						Dexterity = 12,
						Strength = 15,
						ArmorClass = 12
					};

                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);
                    return giantSpider;

                default:
                    throw new ArgumentOutOfRangeException(nameof(monsterID));
            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (DiceService.RollD(100) <= percentage)
            {
                monster.Inventory.AddItem(item: ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
