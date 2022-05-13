using HexagonPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
	public class AreaAbility : Ability
	{
		public int Area = 2;

        public int Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }
        [SerializeField] private int range = default;

        public override List<Hexagon> AquireTarget(Unit unit, HexGrid grid)
        {
            throw new System.NotImplementedException();
        }
    }
}