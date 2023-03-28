using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public abstract class LivingEntity
    {
        public LivingEntity(int id, string name, int dex, int str, int ac,
                           int currentHitPoints, int maximumHitPoints, int gold)
        {
            Id = id;
            Name = name;
            Dexterity = dex;
            Strength = str;
            ArmorClass = ac;
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            Gold = gold;
        }

        public int Id { get; }

        public string Name { get; } = string.Empty;

        public int CurrentHitPoints { get; private set; }

        public int MaximumHitPoints { get; protected set; }

        public int Dexterity { get; } = 10;

        public int Strength { get; } = 10;

        public int ArmorClass { get; } = 10;

        public int Gold { get; private set; }

        public int Level { get; protected set; } = 1;

        public Inventory Inventory { get; } = new Inventory();
        public GameItem? CurrentWeapon { get; set; }

        public bool HasCurrentWeapon => CurrentWeapon != null;
        public GameItem? CurrentConsumable { get; set; }
        public bool HasCurrentConsumable => CurrentConsumable != null;
        public bool IsAlive => CurrentHitPoints > 0;
        public bool IsDead => !IsAlive;
        public void TakeDamage(int hitPointsOfDamage)
        {
            if (hitPointsOfDamage > 0)
            {
                CurrentHitPoints -= hitPointsOfDamage;
            }
        }

        public MessageBox UseCurrentWeaponOn(LivingEntity target)
        {
            if (CurrentWeapon is null)
            {
                throw new InvalidOperationException("CurrentWeapon cannot be null.");
            }

            return CurrentWeapon.PerformAction(this, target);
        }

        public MessageBox UseCurrentConsumable(LivingEntity target)
        {
            if (CurrentConsumable is null)
            {
                throw new InvalidOperationException("CurrentConsumable cannot be null.");
            }

            Inventory.RemoveItem(CurrentConsumable);
            return CurrentConsumable.PerformAction(this, target);
        }

        public void Heal(int hitPointsToHeal)
        {
            if (hitPointsToHeal > 0)
            {
                CurrentHitPoints += hitPointsToHeal;

                if (CurrentHitPoints > MaximumHitPoints)
                {
                    CurrentHitPoints = MaximumHitPoints;
                }
            }
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            if (amountOfGold > 0)
            {
                Gold += amountOfGold;
            }
        }

        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException(nameof(amountOfGold), $"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }

            if (amountOfGold > 0)
            {
                Gold -= amountOfGold;
            }
        }
    }
}
