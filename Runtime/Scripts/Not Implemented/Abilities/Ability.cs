using HexagonPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
	public abstract class Ability : MonoBehaviour
	{
        public int Cooldown
        {
            get
            {
                return cooldown;
            }
            set
            {
                cooldown = value;
            }
        }
        [SerializeField] private int cooldown = 4;

        public int CurrentCooldown
        {
            get
            {
                return currentCooldown;
            }
            set
            {
                if (value > cooldown)
                {
                    value = cooldown;
                }
                currentCooldown = value;
            }
        }
        private int currentCooldown = 0;

        public bool TargetFriendly;

        public bool IsReady()
        {
            if (currentCooldown >= cooldown)
            {
                return true;
            }
            return false;
        }

        public abstract List<Hexagon> AquireTarget(Unit unit, HexagonGrid grid);

        protected List<Hexagon> GetMeleeTargets(Unit unit, HexagonGrid grid)
        {
            List<Cube> neighbours = unit.Hexagon.Cube.GetNeighbours();
            return grid.GetHexagons(neighbours, true);
        }
    }
}