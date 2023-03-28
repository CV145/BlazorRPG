using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
	public class AbilityCalculator
	{
		private const int _defaultAbilityScore = 10;

		public static int CalculateBonus(int score) => (int)Math.Floor((score - _defaultAbilityScore) / 2.0);
	}
}
