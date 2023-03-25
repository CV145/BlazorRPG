using Microsoft.AspNetCore.Components;
using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.ViewModels
{
    public class TraderViewModel
    {
        public Trader? Trader { get; set; } = null;

        public Player? Player { get; set; } = null;

        public string ErrorMessage { get; private set; } = string.Empty;

        public EventCallback InventoryChanged { get; set; }

        public void OnSellItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            if (Player != null && Trader != null)
            {
                Player.Gold += item.Price;
                Trader.Inventory.AddItem(item);
                Player.Inventory.RemoveItem(item);

                InventoryChanged.InvokeAsync(null);
            }
        }

        public void OnBuyItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            if (Player != null && Trader != null)
            {
                ErrorMessage = string.Empty;
                if (Player.Gold >= item.Price)
                {
                    Player.Gold -= item.Price;
                    Trader.Inventory.RemoveItem(item);
                    Player.Inventory.AddItem(item);

                    InventoryChanged.InvokeAsync(null);
                }
                else
                {
                    ErrorMessage = "Error: you do not have enough gold.";
                }
            }
        }
    }
}
