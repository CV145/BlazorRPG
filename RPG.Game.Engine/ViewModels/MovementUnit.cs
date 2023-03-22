using Microsoft.AspNetCore.Components;
using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.ViewModels
{
    public class MovementUnit
    {
        private readonly World world;

        public MovementUnit(World world)
        {
            this.world = world ?? throw new ArgumentNullException(nameof(world));
            this.CurrentLocation = world.GetHomeLocation();
        }

        public Location CurrentLocation { get; private set; }

        public EventCallback<Location> LocationChanged { get; set; }

        public bool CanMoveNorth =>
            this.world.HasLocationAt(this.CurrentLocation.XCoordinate, this.CurrentLocation.YCoordinate + 1);

        public bool CanMoveEast =>
            this.world.HasLocationAt(this.CurrentLocation.XCoordinate + 1, this.CurrentLocation.YCoordinate);

        public bool CanMoveSouth =>
            this.world.HasLocationAt(this.CurrentLocation.XCoordinate, this.CurrentLocation.YCoordinate - 1);

        public bool CanMoveWest =>
            this.world.HasLocationAt(this.CurrentLocation.XCoordinate - 1, this.CurrentLocation.YCoordinate);

        public void MoveNorth() =>
            this.MoveBase(this.CurrentLocation.XCoordinate, this.CurrentLocation.YCoordinate + 1);

        public void MoveEast() =>
            this.MoveBase(this.CurrentLocation.XCoordinate + 1, this.CurrentLocation.YCoordinate);

        public void MoveSouth() =>
            this.MoveBase(this.CurrentLocation.XCoordinate, this.CurrentLocation.YCoordinate - 1);

        public void MoveWest() =>
            this.MoveBase(this.CurrentLocation.XCoordinate - 1, this.CurrentLocation.YCoordinate);

        private void MoveBase(int xCorridate, int yCoordinate)
        {
            if (this.world.HasLocationAt(xCorridate, yCoordinate))
            {
                this.CurrentLocation = this.world.LocationAt(xCorridate, yCoordinate);
                this.LocationChanged.InvokeAsync(this.CurrentLocation);
            }
        }
    }
}
