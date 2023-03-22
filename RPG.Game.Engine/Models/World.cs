using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class World
    {
        private readonly IList<Location> locations;

        //World has locations
        public World(IEnumerable<Location> locs)
        {
            this.locations = locs is null ? new List<Location>() : locs.ToList();
        }

        //Does a location exist at (x,y)?
        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            //LINQ helps find items from collections
            //if loc is null throw an exception, no location exists at
            //given coordinates
            var loc = locations.FirstOrDefault(p => p.XCoordinate == xCoordinate && p.YCoordinate == yCoordinate);
            return loc ?? throw new ArgumentOutOfRangeException("Coordinates", "Provided coordinates could not be found in game world.");

            //?? null coalescing operator
        }

        public bool HasLocationAt(int xCoordinate, int yCoordinate)
        {
            return locations.Any(p => p.XCoordinate == xCoordinate && p.YCoordinate == yCoordinate);
        }

        public Location GetHomeLocation()
        {
            return this.LocationAt(0, -1);
        }
    }
}
