using Exile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
	public enum Team { Friendly, Enemy, Neutral }
	public class Unit : HexObject
	{
		public Team Team;

		public Resource Health;

		public Resource Energy;

		public List<Ability> Abilities = new List<Ability>();

		public void TimeAdvanced()
		{
			foreach (var ability in Abilities)
			{
				ability.CurrentCooldown++;
			}
		}
	}
}